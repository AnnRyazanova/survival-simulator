using System;
using System.Collections;
using UnityEngine;
using Characters.Animations;
using InventoryObjects.Inventory;
using InventoryObjects.Items;
using Objects;

namespace Characters.Controllers
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

        private Vector2 _inputDirections = Vector2.zero;

        private static readonly Quaternion WeaponDockRotation = new Quaternion(0, 0, 0, 0);
        private static readonly Quaternion ToolDockRotation = new Quaternion(0, 0, 180, 0);

        private bool _isInited;

        private void OnTriggerEnter(Collider other) {
            var collidedWith = other.GetComponent<PickableItem>().item;
            var indexToAddTo = inventory.FindFreeCellToAdd(collidedWith);
            if (collidedWith == null || indexToAddTo == -1) return;
            if (other.GetComponent<PickableItem>().isPickable) {
                inventory.AddItem(collidedWith, indexToAddTo);
                Destroy(other.gameObject);
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
            damage = DamageType.Medium;
            StartCoroutine(InitJoystick());
        }

        private void Update() {
            if (_isInited == false) return;
            
            _inputDirections = new Vector2(directionalJoystick.Horizontal, directionalJoystick.Vertical);
            MovementController.Move(transform, characterRotationSpeed, characterMovementSpeed, _inputDirections);
            AnimatorController.OnMove(MovementController.CurrentSpeed / characterMovementSpeed, 0.01f);
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
    }
}