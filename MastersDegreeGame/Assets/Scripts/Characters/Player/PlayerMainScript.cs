using System;
using System.Collections;
using Characters.Animations;
using Characters.Controllers;
using Characters.NPC;
using Characters.Systems;
using Characters.Systems.Combat;
using InventoryObjects.Inventory;
using InventoryObjects.Items;
using Objects;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Player
{
    public class PlayerMainScript : GameCharacter, ICombatAggressor, ICombatTarget
    {
        public FixedJoystick directionalJoystick;
        public PlayerObject playerObject;
        public static PlayerMainScript MyPlayer { get; private set; }

        public GameObject leftHand;
        public GameObject rightHand;

        public Inventory inventory;
        public Equipment equipment;
        public float hitDelaySeconds = 0.5f;
        public ConeRadarSystem coneRadarSystem;
        private Vector2 _inputDirections = Vector2.zero;

        private bool _isInited;

        public void InteractWithClosestItem() {
            var nearest = coneRadarSystem.CheckForVisibleObjects(transform);
            if (nearest != null) {
                var indexToAddTo = inventory.FindFreeCellToAdd(nearest.item);
                if (indexToAddTo == -1) return;
                if (nearest.isPickable) {
                    inventory.AddItem(nearest.item, indexToAddTo);
                    Destroy(nearest.gameObject);
                }
            }
        }

        private void OnApplicationQuit() {
            inventory.Clear();
            equipment.weapon = null;
            equipment.tool = null;
        }

        private void Start() {
            MyPlayer = this;
            MovementController = new ManualMovementController(GetComponent<CharacterController>());
            AnimatorController = new PlayerAnimatorController(GetComponent<Animator>());
            coneRadarSystem = new ConeRadarSystem();
            NavMeshController = new NavMeshController(GetComponent<NavMeshAgent>());
            StartCoroutine(InitJoystick());
        }

        public void Attack() {
            if (equipment.weapon != null) {
                AnimatorController.OnAttackMelee();
                var hits = Physics.OverlapSphere(actionSphere.transform.position, itemSearchRadius);
                if (hits != null) {
                    foreach (var hit in hits) {
                        var colliderParent = hit.gameObject.transform.parent;
                        if (colliderParent != null && colliderParent.GetComponent<NpcMainScript>() != null) {
                            AttackTarget(colliderParent.GetComponent<NpcMainScript>());
                        }
                    }
                }
            }
        }

        public void OnDrawGizmosSelected() {
            Gizmos.DrawWireSphere(actionSphere.transform.position, itemSearchRadius);
        }

        private void Update() {
            if (_isInited == false) return;
            NavMeshController.Move(transform, _inputDirections,
                playerObject.Energy.CurrentPoints > 0 ? characterRunSpeed : characterWalkSpeed);
            AnimatorController.OnMove(_inputDirections.magnitude, playerObject.Energy.CurrentPoints);
        }

        private void FixedUpdate() {
            if (directionalJoystick != null) {
                _inputDirections = new Vector2(directionalJoystick.Horizontal, directionalJoystick.Vertical);
            }
        }

        private IEnumerator InitJoystick() {
            directionalJoystick = MainWindowController.Instance.GetJoystick();

            while (directionalJoystick == null) {
                yield return new WaitForSeconds(.1f);
                directionalJoystick = MainWindowController.Instance.GetJoystick();
            }

            _isInited = true;
        }

        #region EquipmentActions

        private static GameObject InstantiateEquipmentPrefab(GameObject hand, GameObject itemPrefab) {
            var handTransform = hand.transform;
            if (handTransform.childCount > 0) {
                Destroy(handTransform.GetChild(0).gameObject);
            }

            var instance = Instantiate(itemPrefab);
            instance.GetComponent<PickableItem>().isPickable = false;
            return instance;
        }

        private void EquipOnPrefab(GameObject hand, GameObject itemPrefabInstance) {
            itemPrefabInstance.transform.parent = hand.transform;
            itemPrefabInstance.transform.localPosition = Vector3.zero;
            if (hand == rightHand) {
                itemPrefabInstance.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
        }

        private static void UnequipOnPrefab(GameObject hand) {
            foreach (Transform child in hand.transform) {
                Destroy(child.gameObject);
            }
        }

        public void UnequipWeapon() {
            UnequipOnPrefab(rightHand);
        }

        public void UnequipTool() {
            UnequipOnPrefab(leftHand);
        }

        public void EquipWeapon() {
            if (equipment.weapon != null) {
                // Instantiate prefab on scene
                var instance = InstantiateEquipmentPrefab(rightHand, (equipment.weapon as WeaponItem)?.weaponPrefab);
                EquipOnPrefab(rightHand, instance);
                playerObject.Damage.value = (equipment.weapon as WeaponItem).attackPower;
            }
        }

        public void EquipTool() {
            if (equipment.tool != null) {
                // Instantiate prefab on scene
                var instance = InstantiateEquipmentPrefab(leftHand, (equipment.tool as ToolItem)?.toolPrefab);
                EquipOnPrefab(leftHand, instance);
            }
        }

        #endregion

        public void AttackTarget(ICombatTarget target) {
            StartCoroutine(DoDamage(target, hitDelaySeconds));
        }
        
        public IEnumerator DoDamage(ICombatTarget target, float delay) {
            yield return new WaitForSecondsRealtime(delay);
            target.TakeDamage(playerObject.Damage);
        }

        public void TakeDamage(DamageProperty damage) {
            playerObject.Health.AddPoints(-damage.value);
            AnimatorController.OnTakeDamage();
            if (playerObject.Health.CurrentPoints == 0) {
                AnimatorController.OnDie();
                transform.GetChild(0).GetComponent<MeshCollider>().enabled = false;
            }
        }
    }
}