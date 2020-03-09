using System;
using UnityEngine;
using Characters.Animations;

namespace Characters.Controllers
{
    public class PlayerMainScript : GameCharacter
    {
        // !!! Initialization should be made via Unity editor.
        // Should be initialized from UI Canvas Joystick instance
        public FixedJoystick directionalJoystick;

        private Vector2 _inputDirections = Vector2.zero;

        private void Start() {
            MovementController = new ManualMovementController(GetComponent<CharacterController>());
            AnimatorController = new PlayerAnimatorController(GetComponent<Animator>());
            damage = DamageType.Medium;
        }

        private void Update() {
            _inputDirections = new Vector2(directionalJoystick.Horizontal, directionalJoystick.Vertical);
            MovementController.Move(transform, characterRotationSpeed, characterMovementSpeed, _inputDirections);
            AnimatorController.OnMove(MovementController.CurrentSpeed / characterMovementSpeed, 0.01f);
        }
    }
}