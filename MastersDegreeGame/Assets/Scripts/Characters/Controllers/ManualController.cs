using UnityEngine;
using Characters.Animations;

namespace Characters.Controllers
{
    public class ManualController : MonoBehaviour
    {
        [SerializeField] private float characterMovementSpeed = 4f;
        [SerializeField] private float characterRotationSpeed = 0.1f;

        private CharacterController _directionalController = null;
        private PlayerAnimatorController _animatorController;
        
        private Transform _mainCamera = null;
        private Vector3 _moveDirection = Vector3.zero;

        private float _currentSpeed = 0f;
        private float _smoothingVelocity = 0f;
        private const float SmoothingTime = 0.1f;

        // Initialization should be made via Unity editor. Should be initialized from UI Canvas Joystick instance
        public FixedJoystick directionalJoystick;
        private static readonly int MovementSpeedFactor = Animator.StringToHash("movementSpeedFactor");

        private void Start() {
            _directionalController = GetComponent<CharacterController>();
            _animatorController = new PlayerAnimatorController(GetComponent<Animator>());
            
            if (Camera.main != null) _mainCamera = Camera.main.transform;
        }

        private void Update() {
            Move();
        }

        private void Move() {
            var horizontalInput = directionalJoystick.Horizontal;
            var verticalInput = directionalJoystick.Vertical;

            CalculateSpeed(horizontalInput, verticalInput);
            CalculateMovementDirection(horizontalInput, verticalInput,
                _mainCamera.forward.normalized, _mainCamera.right.normalized);

            // Turning the character to face in direction of movement
            if (_moveDirection != Vector3.zero) {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(_moveDirection), characterRotationSpeed);
            }
            _animatorController.OnMove(_currentSpeed / characterMovementSpeed, 0.01f);
            _directionalController.Move(_moveDirection * (_currentSpeed * Time.deltaTime));
        }

        private void CalculateSpeed(float hInput, float vInput) {
            var targetSpeed = characterMovementSpeed * Mathf.Sqrt(hInput * hInput + vInput * vInput);
            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, targetSpeed,
                ref _smoothingVelocity, SmoothingTime);
        }

        private void CalculateMovementDirection(float hInput, float vInput,
                                                Vector3 fwdCameraDirection, Vector3 sdwCameraDirection) {
            _moveDirection = (fwdCameraDirection * vInput + sdwCameraDirection * hInput).normalized;
            // Set Y coordinate to zero to prevent character from tilting along that axis
            _moveDirection.y = 0;
        }
    }
}