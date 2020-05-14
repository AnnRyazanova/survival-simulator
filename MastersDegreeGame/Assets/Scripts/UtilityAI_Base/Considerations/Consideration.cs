using System;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;
using UtilityAI_Base.Considerations.Interfaces;
using UtilityAI_Base.Contexts.Interfaces;
using UtilityAI_Base.ResponseCurves;
using UtilityAI_Base.ResponseCurves.Interfaces;
using UtilityAI_Base.ResponseCurves.SuppliedCurves;
using Object = System.Object;

namespace UtilityAI_Base.Considerations
{
    /// <summary>
    /// Abstract Consideration implementation
    /// All considerations should implement this class instead of an interface 
    /// </summary>
    [Serializable]
    public class Consideration
    {
        public string description = "consideration";
        [SerializeField] private bool isEnabled = true;

        /// <summary>
        /// Utility curve to evaluate current consideration
        /// </summary>
        public ResponseCurve utilityCurve;
        public int curveId = 0;

        
        public void Awake() {
            utilityCurve = new LinearResponseCurve();
        }

        public float Evaluate(IAiContext context) {
            throw new System.NotImplementedException();
        }
    }
}