using System;
using UnityEngine;
using Characters.Animations;

namespace Characters.Controllers
{
    public class PlayerMainScript : MonoBehaviour
    {
        [SerializeField] private float characterMovementSpeed = 4f;
        [SerializeField] private float characterRotationSpeed = 0.1f;

        private PlayerAnimatorController _animatorController;

        private ManualMovementController _movementController;

        // !!! Initialization should be made via Unity editor.
        // Should be initialized from UI Canvas Joystick instance
        public FixedJoystick directionalJoystick;
        
        private Vector2 _inputDirections = Vector2.zero;
        
        private void Start() {
            _movementController = new ManualMovementController(GetComponent<CharacterController>());
            _animatorController = new PlayerAnimatorController(GetComponent<Animator>());
        }

        private void Update() {
            _inputDirections = new Vector2(directionalJoystick.Horizontal, directionalJoystick.Vertical);
            _movementController.Move(transform, characterRotationSpeed, characterMovementSpeed, _inputDirections);
            _animatorController.OnMove(_movementController.CurrentSpeed / characterMovementSpeed, 0.01f);
        }
    }
}