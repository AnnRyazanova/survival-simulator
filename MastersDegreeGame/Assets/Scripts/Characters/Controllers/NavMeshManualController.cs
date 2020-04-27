using System;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Controllers
{
    public class NavMeshController
    {
        protected NavMeshAgent Agent;
        protected const float SmoothingTime = 0.1f;

        protected float SmoothingVelocity;
        public Vector3 Movement = Vector3.zero;
        public float CurrentSpeed { get; set; }


        public NavMeshController(NavMeshAgent agent, float speed = 4f, float angularSpeed = 0f, 
                        float acceleration = 0f) {
            Agent = agent;
            Agent.speed = speed;
            Agent.acceleration = acceleration;
            Agent.angularSpeed = angularSpeed;
        }

        protected void CalculateMovementParameters(float characterMovementSpeed, Vector2 inputDirection) {
            // Calculate character target speed
            var targetSpeed = characterMovementSpeed * inputDirection.magnitude;
            // Gradually change speed from current towards desired (target speed)
            CurrentSpeed = Mathf.SmoothDamp(CurrentSpeed, targetSpeed,
                ref SmoothingVelocity, SmoothingTime);

            // Calculate moving direction
            Movement = (Vector3.forward * inputDirection.y + Vector3.right * inputDirection.x).normalized;
            // Set Y coordinate to zero to prevent character from tilting along that axis
            Movement.y = 0;
        }

        public void Move(Transform transform, Vector2 inputDirection, float speed) {
            Agent.speed = speed;
            CalculateMovementParameters(Agent.speed, inputDirection);
            if (Movement != Vector3.zero) {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(Movement), 0.1f);
            }

            Agent.Move(Movement * (Time.deltaTime * CurrentSpeed));
            Agent.SetDestination(Movement + transform.position);
        }
    }
}