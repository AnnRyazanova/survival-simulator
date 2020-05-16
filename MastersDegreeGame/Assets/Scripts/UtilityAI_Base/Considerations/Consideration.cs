using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UtilityAI_Base.Contexts.Interfaces;
using UtilityAI_Base.ResponseCurves;

using UtilityAI_Base.ResponseCurves.SuppliedCurves;

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

        public Dictionary<CurveType, ResponseCurve> Curves = new Dictionary<CurveType, ResponseCurve>();
        
        /// <summary>
        /// Utility curve to evaluate current consideration
        /// </summary>
        public ResponseCurve utilityCurve;
        public int curveId = 0;
        public CurveType responseCurveType = CurveType.Linear;
        
        public Consideration() {
            utilityCurve = new LinearResponseCurve();
            Curves.Add(CurveType.Linear, new LinearResponseCurve());
            Curves.Add(CurveType.Quadratic, new QuadraticResponseCurve());
            Curves.Add(CurveType.Logistic, new LogisticResponseCurve());
        }

        public float Evaluate(IAiContext context) {
            throw new System.NotImplementedException();
        }
    }
}