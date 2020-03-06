using UnityEngine;

namespace Characters.Animations
{
    public class PlayerAnimatorController
    {
        private readonly Animator _animatorController;

        public PlayerAnimatorController(Animator animatorController) => _animatorController = animatorController;
        
        private static readonly int MovementSpeedFactor = Animator.StringToHash("movementSpeedFactor");
        
        public void OnMove(float speedFactor, float smoothingFactor) {
            _animatorController.SetFloat(MovementSpeedFactor, speedFactor, 
                smoothingFactor, Time.deltaTime);
            
        }
    }
}