using UnityEngine;

namespace Characters.Animations
{
    public abstract class CharacterAnimationController 
    {
        protected readonly Animator AnimatorController;

        protected CharacterAnimationController(Animator animatorController) => AnimatorController = animatorController;

        public abstract void OnMove(float speedFactor, float smoothingFactor);
        
        public abstract void OnAttackMelee();
    }
}
