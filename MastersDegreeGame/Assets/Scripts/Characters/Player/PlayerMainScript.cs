using System;
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

        private void OnTriggerEnter(Collider other) {
            Debug.Log("Entered collision");
            var collidedWith = other.GetComponent<PickableItem>().item;
            if (collidedWith == null) return;
            if (other.GetComponent<PickableItem>().isPickable) {
                inventory.AddItem(collidedWith);
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
            InitInventory();
            InitJoystick();
        }

        private void Update() {
            _inputDirections = new Vector2(directionalJoystick.Horizontal, directionalJoystick.Vertical);
            MovementController.Move(transform, characterRotationSpeed, characterMovementSpeed, _inputDirections);
            AnimatorController.OnMove(MovementController.CurrentSpeed / characterMovementSpeed, 0.01f);
        }

        private static GameObject SetEquippedItemProps(GameObject hand, GameObject itemPrefab) {
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
            itemPrefabInstance.transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        public void EquipWeapon() {
            if (equipment.weapon != null) {
                // Instantiate prefab on scene
                var instance = SetEquippedItemProps(rightHand, equipment.weapon.weaponPrefab);
                EquipOnPrefab(rightHand, instance);
            }
        }

        public void EquipTool() {
            if (equipment.weapon != null) {
                // Instantiate prefab on scene
                var instance = SetEquippedItemProps(rightHand, equipment.tool.toolPrefab);
                EquipOnPrefab(rightHand, instance);
            }
        }

        private void InitInventory() {
            for (var i = 0; i < inventory.maxLength; ++i) {
                inventory.container.Add(new InventoryCell(null, 0));
            }
        }

        private void InitJoystick() {
            directionalJoystick = MainWindowController.Instance.GetJoystick();
        }
    }
}