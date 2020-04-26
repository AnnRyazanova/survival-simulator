#define DISPLAY_GIZMOS

using Characters.Animations;
using Characters.Controllers;
using UnityEngine;
using UnityEngine.Serialization;


namespace Characters
{
    public abstract class GameCharacter : MonoBehaviour
    {
        [SerializeField] protected float characterMovementSpeed = 4f;
        [SerializeField] protected float characterRotationSpeed = 0.1f;
        public float attackRate = 1.5f;
        
        public float lastAttackTime = 0.0f;
        
        protected CharacterAnimationController AnimatorController;
        protected MovementController MovementController;
        /// <summary>
        /// Character action/attack hitbox
        /// </summary>
        public GameObject actionSphere;
        
        [SerializeField] protected float radius = 1.0f;

#if DISPLAY_GIZMOS     
        public void OnDrawGizmosSelected() {
            Gizmos.DrawWireSphere(actionSphere.transform.position, radius);
        }
#endif

    }
}