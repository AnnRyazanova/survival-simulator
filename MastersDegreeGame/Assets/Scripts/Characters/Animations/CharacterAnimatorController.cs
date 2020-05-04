using UnityEngine;

namespace Characters.Animations
{
    public abstract class CharacterAnimatorController
    {
        protected readonly Animator AnimatorController;

        protected static readonly int MovementSpeedFactor = Animator.StringToHash("movementSpeedFactor");
        protected static readonly int AttackMelee = Animator.StringToHash("attackMelee");
        protected static readonly int AttackRanged = Animator.StringToHash("attackRanged");
        protected static readonly int TakeDamage = Animator.StringToHash("takeDamage");
        protected static readonly int Die = Animator.StringToHash("isDead");
        protected static readonly int Move = Animator.StringToHash("move");
        protected static readonly int OnlyWalk = Animator.StringToHash("onlyWalk");


        protected CharacterAnimatorController(Animator animatorController) => AnimatorController = animatorController;

        public abstract void OnMove(float speedFactor, int energy);

        public abstract void OnAttackMelee();

        public abstract void OnAttackRanged();

        public abstract void OnTakeDamage();

        public abstract void OnDie();
    }
}