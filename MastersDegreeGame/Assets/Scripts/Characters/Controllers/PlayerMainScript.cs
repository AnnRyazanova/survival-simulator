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

        private void Start() {
            _movementController =
                new ManualMovementController(GetComponent<CharacterController>(), directionalJoystick);
            _animatorController = new PlayerAnimatorController(GetComponent<Animator>());
        }

        private void Update() {
            _movementController.CalculateMovementParameters(characterMovementSpeed);
            _animatorController.OnMove(_movementController.CurrentSpeed / characterMovementSpeed, 0.01f);
        }

        private void FixedUpdate() {
            _movementController.Move(transform, characterRotationSpeed);
        }
    }
}