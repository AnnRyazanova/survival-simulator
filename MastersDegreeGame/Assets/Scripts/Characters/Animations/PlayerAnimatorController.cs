﻿using UnityEngine;

namespace Characters.Animations
{
    public class PlayerAnimatorController: CharacterAnimationController
    {
        public PlayerAnimatorController(Animator animatorController) : base(animatorController){}
        
        private static readonly int MovementSpeedFactor = Animator.StringToHash("movementSpeedFactor");
        
        public override void OnMove(float speedFactor, float smoothingFactor) {
            AnimatorController.SetFloat(MovementSpeedFactor, speedFactor, 
                smoothingFactor, Time.deltaTime);
        }
    }
}