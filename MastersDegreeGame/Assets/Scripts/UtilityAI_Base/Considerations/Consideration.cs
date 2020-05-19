using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Serialization;
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
        [SerializeField] private string description = "consideration";
        [SerializeField] private bool isEnabled = true;
        
        [HideInInspector]
        public int evaluatedContextVariableId = 0;
       
        public string evaluatedContextVariable = null;
        public CurveType responseCurveType = CurveType.Linear;
        public Dictionary<CurveType, ResponseCurve> Curves = new Dictionary<CurveType, ResponseCurve>()
        {
            {
                CurveType.Linear, new LinearResponseCurve()
            },
            {
                CurveType.Quadratic, new QuadraticResponseCurve()
            },

            {
                CurveType.Logistic, new LogisticResponseCurve()
            }
        };

        /// <summary>
        /// Utility curve to evaluate current consideration
        /// </summary>
        public ResponseCurve utilityCurve
        {
            get => Curves[responseCurveType];
            set => Curves[responseCurveType] = value;
        }
        
        public Consideration() {
            utilityCurve = new LinearResponseCurve();
        }

        public float Evaluate(IAiContext context) {
            return evaluatedContextVariable != null
                ? utilityCurve.EvaluateAt(context.GetParameter(evaluatedContextVariable))
                : 0f;
        }
    }
}