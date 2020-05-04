using UnityEngine;

namespace Characters.Animations
{
    public class NpcAnimatorController : CharacterAnimatorController
    {
        public NpcAnimatorController(Animator animatorController) : base(animatorController){}
        
        public override void OnMove(float speedFactor, int energy) {
            AnimatorController.SetBool(Move, speedFactor > 0);
        }

        public override void OnAttackMelee() {
            AnimatorController.SetTrigger(AttackMelee);
        }

        public override void OnAttackRanged() {
            throw new System.NotImplementedException();
        }

        public override void OnTakeDamage() {
            AnimatorController.SetTrigger(TakeDamage);
        }

        public override void OnDie() {
            AnimatorController.SetBool(Die, true);
        }
    }
}