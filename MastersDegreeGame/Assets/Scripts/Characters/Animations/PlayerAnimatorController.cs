using UnityEngine;

namespace Characters.Animations
{
    public class PlayerAnimatorController: CharacterAnimationController
    {
        public PlayerAnimatorController(Animator animatorController) : base(animatorController){}
        
        public override void OnMove(float speedFactor, int energy) {
            // AnimatorController.SetFloat(MovementSpeedFactor, speedFactor, 
            //     smoothingFactor, Time.deltaTime);
            AnimatorController.SetBool(OnlyWalk, energy == 0);
            AnimatorController.SetBool(Move, speedFactor > 0.0f);
        }

        public override void OnAttackMelee() {
            AnimatorController.SetTrigger(AttackMelee);
        }

        public override void OnTakeDamage() {
            throw new System.NotImplementedException();
        }

        public override void OnDie() {
            throw new System.NotImplementedException();
        }
    }
}