using System;
using AI.Contexts;
using Characters.Controllers;
using Characters.Player;
using UnityEditor.Experimental.UIElements;
using UnityEngine;
using UnityEngine.Animations;

namespace AI.Sensors
{
    public sealed class RadialSensor : MonoBehaviour
    {
        public float visibilityRadius;
        public int colliderBufferSize;
        public LayerMask visibleLayers;

        public AiContext context;
        private float tmp;
        
        private Collider[] _detectedColliders;
        private int _detectedCollidersCount;

        #region Unity functions

        private void Awake() {
            colliderBufferSize = colliderBufferSize == 0 ? 10 : colliderBufferSize;
            _detectedColliders = new Collider[colliderBufferSize];
            context = new AiContext();
            context.target = null;
        }

        private void Update() {
            if (_detectedCollidersCount > 0) {
                foreach (var detectedCollider in _detectedColliders) {
                    if (detectedCollider != null) {
                        var c = detectedCollider.gameObject.GetComponent<PlayerMainScript>();
                        if ( c != null) {
                            context.target = c;
                            return;
                        }

                        context.target = null;
                    }
                }
            }
            
            if (Time.time >= tmp && context.target != null) {
                tmp = Time.time + 1 / 2f;
                Debug.Log((context.target as PlayerMainScript).name);
            }
        }

        private void FixedUpdate() {
            _detectedCollidersCount = Physics.OverlapSphereNonAlloc(transform.position, visibilityRadius,
                _detectedColliders, visibleLayers);
        }

        #endregion
    }
}