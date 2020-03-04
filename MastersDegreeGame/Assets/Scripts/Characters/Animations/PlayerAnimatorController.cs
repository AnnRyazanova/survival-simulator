using UnityEngine;

namespace Characters.Animations
{
    public class PlayerAnimatorController
    {
        private Animator _animatorController;

        public PlayerAnimatorController(Animator animatorController) => _animatorController = animatorController;
        private static readonly int MovementSpeedFactor = Animator.StringToHash("movementSpeedFactor");
        private static readonly int ShouldRun = Animator.StringToHash("shouldRun");
        private static readonly int ShouldWalk = Animator.StringToHash("shouldWalk");
        private static readonly int ShouldIdle = Animator.StringToHash("shouldIdle");
        public void OnMove(float speedFactor, float smoothingFactor) {
            _animatorController.SetFloat(MovementSpeedFactor, speedFactor, 
                smoothingFactor, Time.deltaTime);
            
        }
    }
}