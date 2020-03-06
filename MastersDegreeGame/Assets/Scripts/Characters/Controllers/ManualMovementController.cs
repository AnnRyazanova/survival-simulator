using UnityEngine;

namespace Characters.Controllers
{
    public class ManualMovementController
    {
        private readonly CharacterController _directionalController;
        private readonly Transform _mainCamera;

        private const float Gravity = 9.8f;
        private const float SmoothingTime = 0.1f;

        private float _smoothingVelocity;
        private Vector3 _moveDirection = Vector3.zero;

        public float CurrentSpeed { get; private set; }

        public ManualMovementController(CharacterController directionalController) {
            _directionalController = directionalController;
            if (Camera.main != null) _mainCamera = Camera.main.transform;
        }

        private void CalculateMovementParameters(float characterMovementSpeed, Vector2 inputDirection) {
            // Calculate character target speed
            var targetSpeed = characterMovementSpeed * inputDirection.magnitude;
            // Gradually change speed from current towards desired (target speed)
            CurrentSpeed = Mathf.SmoothDamp(CurrentSpeed, targetSpeed,
                ref _smoothingVelocity, SmoothingTime);

            // Calculate moving direction
            _moveDirection = (_mainCamera.forward * inputDirection.y + _mainCamera.right * inputDirection.x).normalized;
            // Set Y coordinate to zero to prevent character from tilting along that axis
            _moveDirection.y = 0;
        }

        public void Move(Transform transform, float characterRotationSpeed, float characterMovementSpeed,
            Vector2 inputDirection) {
            CalculateMovementParameters(characterMovementSpeed, inputDirection);
            // Apply gravity if the character is not on the ground 
            if (!_directionalController.isGrounded) {
                _directionalController.Move(new Vector3(0, -Gravity, 0) * Time.deltaTime);
            }

            if (_moveDirection != Vector3.zero) {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(_moveDirection), characterRotationSpeed);
            }

            // Move the character using computed values for speed and direction
            _directionalController.Move(_moveDirection * (CurrentSpeed * Time.deltaTime));
        }
    }
}