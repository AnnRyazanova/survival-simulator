using Characters.Animations;
using Characters.Controllers;
using UnityEngine;

namespace Characters
{
    public abstract class GameCharacter : MonoBehaviour
    {
        [SerializeField] protected float characterMovementSpeed = 4f;
        [SerializeField] protected float characterRotationSpeed = 0.1f;
        
        protected PlayerAnimatorController AnimatorController;
        protected MovementController MovementController;
    }
}