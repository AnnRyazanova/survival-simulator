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

        public Inventory inventory;
        
        private Vector2 _inputDirections = Vector2.zero;

        private void OnTriggerEnter(Collider other) {
            Debug.Log("Entered collision");
            var collidedWith = other.GetComponent<PickableItem>().item;
            if (collidedWith == null) return;
            
            inventory.AddItem(collidedWith);
            if (collidedWith.ItemType != ItemObjectType.Weapon) {
                Destroy(other.gameObject);
            }
        }

        private void OnApplicationQuit() {
            inventory.Clear();
        }

        private void Start()
        {
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

        private void InitInventory() {
            for (var i = 0; i < inventory.maxLength; ++i) {
                inventory.container.Add(null);
            }
        }
        
        private void InitJoystick()
        {
            directionalJoystick = MainWindowController.Instance.GetJoystick();
        }
    }
}