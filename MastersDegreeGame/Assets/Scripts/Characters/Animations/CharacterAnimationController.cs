using UnityEngine;

namespace Characters.Animations
{
    public abstract class CharacterAnimationController 
    {
        protected readonly Animator AnimatorController;
        
        protected static readonly int MovementSpeedFactor = Animator.StringToHash("movementSpeedFactor");
        protected static readonly int AttackMelee = Animator.StringToHash("attackMelee");

        protected CharacterAnimationController(Animator animatorController) => AnimatorController = animatorController;

        public abstract void OnMove(float speedFactor, float smoothingFactor);
        
        public abstract void OnAttackMelee();
    }
}
