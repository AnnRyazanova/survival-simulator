using Characters.Animations;
using Characters.Controllers;
using UnityEngine;

namespace Characters
{
    public enum DamageType
    {
        None = 0,
        Low = 5,
        Medium = 15,
        High = 25
    }

    public abstract class GameCharacter : MonoBehaviour
    {
        [SerializeField] protected float characterMovementSpeed = 4f;
        [SerializeField] protected float characterRotationSpeed = 0.1f;
        [SerializeField]  protected DamageType damage = DamageType.None;

        public int health = 100;
        public int fullness = 100;

        protected PlayerAnimatorController AnimatorController;
        protected MovementController MovementController;
    }
}