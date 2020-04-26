﻿#define DISPLAY_GIZMOS

using Characters.Animations;
using Characters.Controllers;
using UnityEngine;
using UnityEngine.Serialization;


namespace Characters
{
    public abstract class GameCharacter : MonoBehaviour
    {
        [SerializeField] protected float characterRunSpeed = 2f;
        [SerializeField] protected float characterWalkSpeed = .9f;
        public float attackRate = 1.5f;
        
        public float lastAttackTime = 0.0f;
        
        protected CharacterAnimatorController AnimatorController;
        protected MovementController MovementController;
        protected NavMeshController NavMeshController;
        
        /// <summary>
        /// Character action/attack hitbox
        /// </summary>
        public GameObject actionSphere;
        
        [FormerlySerializedAs("radius")] [SerializeField] protected float itemSearchRadius = 1.0f;

#if DISPLAY_GIZMOS     
        public void OnDrawGizmosSelected() {
            Gizmos.DrawWireSphere(actionSphere.transform.position, itemSearchRadius);
        }
#endif

    }
}