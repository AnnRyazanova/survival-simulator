using UnityEngine;

namespace Characters.Animations
{
    public class PlayerAnimatorController: CharacterAnimationController
    {
        public PlayerAnimatorController(Animator animatorController) : base(animatorController){}
        
        public override void OnMove(float speedFactor, float smoothingFactor) {
            AnimatorController.SetFloat(MovementSpeedFactor, speedFactor, 
                smoothingFactor, Time.deltaTime);
        }

        public override void OnAttackMelee() {
            AnimatorController.SetTrigger(AttackMelee);
        }
    }
}