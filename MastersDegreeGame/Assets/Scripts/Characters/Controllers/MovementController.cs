using UnityEngine;

namespace Characters.Controllers
{
    public abstract class MovementController
    {
        protected readonly CharacterController DirectionalController;

        protected const float Gravity = 9.8f;
        protected const float SmoothingTime = 0.1f;

        protected float SmoothingVelocity;
        protected Vector3 MoveDirection = Vector3.zero;

        public float CurrentSpeed { get; protected set; }

        protected MovementController(CharacterController directionalController) {
            DirectionalController = directionalController;
        }

        protected abstract void CalculateMovementParameters(float characterMovementSpeed, Vector2 inputDirection);

        public abstract void Move(Transform transform, float characterRotationSpeed, float characterMovementSpeed,
            Vector2 inputDirection);
    }
}