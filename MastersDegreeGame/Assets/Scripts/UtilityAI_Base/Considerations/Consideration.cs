using System;
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
    // [CreateAssetMenu(fileName = "New Consideration", menuName = "Utility_AI/Consideration")]
    public sealed class Consideration
    {
        public string trata = "sosi";
        /// <summary>
        /// Utility curve to evaluate current consideration
        /// </summary>
        public ResponseCurve utilityCurve;

        public void Awake() {
            utilityCurve = new LinearResponseCurve();
        }

        public float Evaluate(IAiContext context) {
            throw new System.NotImplementedException();
        }
    }
}