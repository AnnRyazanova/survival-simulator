using System;
using UnityEngine;
using Characters.Animations;

namespace Characters.Controllers
{
    public class PlayerMainScript : GameCharacter
    {
        public FixedJoystick directionalJoystick;

        private Vector2 _inputDirections = Vector2.zero;

        private void Start() {
            MovementController = new ManualMovementController(GetComponent<CharacterController>());
            AnimatorController = new PlayerAnimatorController(GetComponent<Animator>());
            damage = DamageType.Medium;
            InitJoystick();
        }

        private void Update() {
            _inputDirections = new Vector2(directionalJoystick.Horizontal, directionalJoystick.Vertical);
            MovementController.Move(transform, characterRotationSpeed, characterMovementSpeed, _inputDirections);
            AnimatorController.OnMove(MovementController.CurrentSpeed / characterMovementSpeed, 0.01f);
        }
        
        private void InitJoystick()
        {
            directionalJoystick = MainWindowController.Instance.GetJoystick();
        }
    }
}