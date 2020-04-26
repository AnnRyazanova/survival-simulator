using UnityEngine;

namespace Characters.Animations
{
    public class NpcAnimationController : CharacterAnimationController
    {
        public NpcAnimationController(Animator animatorController) : base(animatorController){}
        
        public override void OnMove(float speedFactor, float smoothingFactor) {
            AnimatorController.SetFloat(MovementSpeedFactor, speedFactor, 
                smoothingFactor, Time.deltaTime);
        }

        public override void OnAttackMelee() {
            AnimatorController.SetTrigger(AttackMelee);
        }

        public override void OnTakeDamage() {
            AnimatorController.SetTrigger(TakeDamage);
        }

        public override void OnDie() {
            AnimatorController.SetBool(Die, true);
        }
    }
}