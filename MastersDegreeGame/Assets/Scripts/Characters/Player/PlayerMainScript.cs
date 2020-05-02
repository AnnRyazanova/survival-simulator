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
    public class PlayerMainScript : GameCharacter
    {
        public FixedJoystick directionalJoystick;
        public PlayerObject playerObject;
        public static PlayerMainScript MyPlayer { get; private set; }


        public GameObject leftHand;
        public GameObject rightHand;

        public Inventory inventory;
        public Equipment equipment;
        public float hitDelaySeconds = 0.1f;
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

        private void Awake() {
            coneRadarSystem = new ConeRadarSystem();
            animatorController = new PlayerAnimatorController(GetComponent<Animator>());
            NavMeshController = new NavMeshController(GetComponent<NavMeshAgent>());
            StartCoroutine(InitJoystick());
        }

        private void Start() {
            MyPlayer = this;
            playerObject.animatorController = animatorController;
        }

        #region Combat

        public void Attack() {
            if (equipment.weapon.item != null) {
                animatorController.OnAttackMelee();
                var hits = Physics.OverlapSphere(actionSphere.transform.position, itemSearchRadius);
                if (hits != null) {
                    foreach (var hit in hits) {
                        var colliderParent = hit.gameObject.transform.parent;
                        if (colliderParent != null) {
                            if (colliderParent.GetComponent<NpcMainScript>() != null) {
                                playerObject.AttackTarget(colliderParent.GetComponent<NpcMainScript>().npcObject);
                            }
                        }
                    }
                }
            }
        }

        #endregion


            public void OnDrawGizmosSelected() {
                Gizmos.DrawWireSphere(actionSphere.transform.position, itemSearchRadius);
            }

            private void Update() {
                if (_isInited == false) return;
                _inputDirections = new Vector2(directionalJoystick.Horizontal, directionalJoystick.Vertical);
                NavMeshController.Move(transform, _inputDirections,
                    playerObject.Energy.CurrentPoints > 0 ? characterRunSpeed : characterWalkSpeed);
                animatorController.OnMove(_inputDirections.magnitude, playerObject.Energy.CurrentPoints);
                inventory.UpdateItems();
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
                    var instance =
                        InstantiateEquipmentPrefab(rightHand, (equipment.weapon.item as WeaponItem)?.weaponPrefab);
                    EquipOnPrefab(rightHand, instance);
                    playerObject.Damage.value = (equipment.weapon.item as WeaponItem).attackPower;
                }
            }

            public void EquipTool() {
                if (equipment.tool != null) {
                    // Instantiate prefab on scene
                    var instance = InstantiateEquipmentPrefab(leftHand, (equipment.tool.item as ToolItem)?.toolPrefab);
                    EquipOnPrefab(leftHand, instance);
                }
            }

            #endregion
        }
    }