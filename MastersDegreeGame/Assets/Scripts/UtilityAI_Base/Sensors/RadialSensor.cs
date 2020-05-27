using System;
using UnityEngine;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.Contexts.Interfaces;

namespace UtilityAI_Base.Sensors
{
    public sealed class RadialSensor : MonoBehaviour, IContextProvider
    {
        #region public fields

        public float visibilityRadius;
        public int colliderBufferSize;

        public LayerMask visibleLayers;

        public AiContext context;

        #endregion

        #region private fields

        private Collider[] _detectedCollidersBuffer;
        private int _detectedCollidersCount;

        #endregion
        
        #region interface methods implementations
        
        public void ProvideContext(IAiContext context) {
            throw new NotImplementedException();
        }
        
        #endregion
        
        #region Unity functions

        private void Awake() {
            colliderBufferSize = colliderBufferSize == 0 ? 10 : colliderBufferSize;
            _detectedCollidersBuffer = new Collider[colliderBufferSize];
        }

        private void Update() {
            if (_detectedCollidersCount > 0) {
                ProvideContext(context);
            }
        }

        private void FixedUpdate() {
            _detectedCollidersCount = Physics.OverlapSphereNonAlloc(transform.position, visibilityRadius,
                _detectedCollidersBuffer, visibleLayers);
        }

        #endregion

        #region user defined context actions
        
        #endregion
    }
}