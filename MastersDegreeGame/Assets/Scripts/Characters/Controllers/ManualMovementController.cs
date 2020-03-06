using UnityEngine;

namespace Characters.Controllers
{
    public class ManualMovementController
    {
        private readonly CharacterController _directionalController;
        private readonly FixedJoystick _directionalJoystick;
        private readonly Transform _mainCamera;

        private const float Gravity = 9.8f;
        private const float SmoothingTime = 0.1f;

        private float _smoothingVelocity;
        private Vector3 _moveDirection = Vector3.zero;

        public float CurrentSpeed { get; private set; }

        public ManualMovementController(CharacterController directionalController, FixedJoystick directionalJoystick) {
            _directionalController = directionalController;
            _directionalJoystick = directionalJoystick;
            if (Camera.main != null) _mainCamera = Camera.main.transform;
        }

        public void CalculateMovementParameters(float characterMovementSpeed) {
            // Directional input components
            var horizontalInput = _directionalJoystick.Horizontal;
            var verticalInput = _directionalJoystick.Vertical;
            var dirMagnitude = Mathf.Sqrt(horizontalInput * horizontalInput + verticalInput * verticalInput);

            // Calculate character target speed
            var targetSpeed = characterMovementSpeed * dirMagnitude;
            // Gradually change speed from current towards desired (target speed)
            CurrentSpeed = Mathf.SmoothDamp(CurrentSpeed, targetSpeed,
                ref _smoothingVelocity, SmoothingTime);

            // Calculate moving direction
            _moveDirection = (_mainCamera.forward * verticalInput + _mainCamera.right * horizontalInput).normalized;
            // Set Y coordinate to zero to prevent character from tilting along that axis
            _moveDirection.y = 0;
        }

        public void Move(Transform transform, float characterRotationSpeed) {
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