using UnityEngine;
using Characters.Animations;

namespace Characters.Controllers
{
    public class ManualController : MonoBehaviour
    {
        [SerializeField] private float characterMovementSpeed = 4f;
        [SerializeField] private float characterRotationSpeed = 0.1f;
        [SerializeField] private float gravity = 9.8f;

        private CharacterController _directionalController;
        private PlayerAnimatorController _animatorController;

        private Transform _mainCamera;
        private Vector3 _moveDirection = Vector3.zero;

        private float _currentSpeed;
        private float _smoothingVelocity;
        private const float SmoothingTime = 0.1f;

        // Initialization should be made via Unity editor. Should be initialized from UI Canvas Joystick instance
        public FixedJoystick directionalJoystick;

        private void Start() {
            _directionalController = GetComponent<CharacterController>();
            _animatorController = new PlayerAnimatorController(GetComponent<Animator>());

            if (Camera.main != null) _mainCamera = Camera.main.transform;
        }

        private void Update() {
            Move();
        }

        private void Move() {
            CalculateMovementParameters();
            TurnAndAnimateOnMove();
            // Apply gravity if the character is not on the ground 
            if (!_directionalController.isGrounded) {
                _directionalController.Move(new Vector3(0, -gravity, 0) * Time.deltaTime);
            }
            // Move the character using computed values for speed and direction
            _directionalController.Move(_moveDirection * (_currentSpeed * Time.deltaTime));
        }
        
        private void TurnAndAnimateOnMove() {
            // Turning the character to face in direction of movement
            if (_moveDirection != Vector3.zero) {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(_moveDirection), characterRotationSpeed);
            }

            // Trigger movement animation
            _animatorController.OnMove(_currentSpeed / characterMovementSpeed, 0.01f);
        }

        private void CalculateMovementParameters() {
            // Directional input components
            var horizontalInput = directionalJoystick.Horizontal;
            var verticalInput = directionalJoystick.Vertical;
            var dirMagnitude = Mathf.Sqrt(horizontalInput * horizontalInput + verticalInput * verticalInput);
            
            // Calculate character target speed
            var targetSpeed = characterMovementSpeed * dirMagnitude;
            // Gradually change speed from current towards desired (target speed)
            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, targetSpeed,
                ref _smoothingVelocity, SmoothingTime);
            
            // Calculate moving direction
            _moveDirection = (_mainCamera.forward * verticalInput + _mainCamera.right * horizontalInput).normalized;
            // Set Y coordinate to zero to prevent character from tilting along that axis
            _moveDirection.y = 0;
        }
    }
}