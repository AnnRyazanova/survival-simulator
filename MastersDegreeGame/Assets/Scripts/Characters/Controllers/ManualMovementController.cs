using UnityEngine;

namespace Characters.Controllers
{
    public class ManualMovementController: MovementController
    {
        private readonly Transform _mainCamera;
        
        public ManualMovementController(CharacterController directionalController) : base(directionalController) {
            if (Camera.main != null) _mainCamera = Camera.main.transform;
        }

        protected override void CalculateMovementParameters(float characterMovementSpeed, Vector2 inputDirection) {
            // Calculate character target speed
            var targetSpeed = characterMovementSpeed * inputDirection.magnitude;
            // Gradually change speed from current towards desired (target speed)
            CurrentSpeed = Mathf.SmoothDamp(CurrentSpeed, targetSpeed,
                ref SmoothingVelocity, SmoothingTime);

            // Calculate moving direction
            MoveDirection = (_mainCamera.forward * inputDirection.y + _mainCamera.right * inputDirection.x).normalized;
            // Set Y coordinate to zero to prevent character from tilting along that axis
            MoveDirection.y = 0;
        }

        public override void Move(Transform transform, float characterRotationSpeed, float characterMovementSpeed,
            Vector2 inputDirection) {
            CalculateMovementParameters(characterMovementSpeed, inputDirection);
            // Apply gravity if the character is not on the ground 
            if (!DirectionalController.isGrounded) {
                DirectionalController.Move(new Vector3(0, -Gravity, 0) * Time.deltaTime);
            }

            if (MoveDirection != Vector3.zero) {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(MoveDirection), characterRotationSpeed);
            }

            // Move the character using computed values for speed and direction
            DirectionalController.Move(MoveDirection * (CurrentSpeed * Time.deltaTime));
        }
    }
}