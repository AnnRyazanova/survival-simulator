using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityAI_Base.Contexts.Interfaces;
using UtilityAI_Base.ResponseCurves;
using UtilityAI_Base.ResponseCurves.SuppliedCurves;

namespace UtilityAI_Base.Considerations
{
    /// <summary>
    /// Consideration implementation
    /// All considerations should implement this class instead of an interface 
    /// </summary>
    [Serializable]
    public class Consideration
    {
        [SerializeField] private string description = "New consideration";
        [SerializeField] private bool isEnabled = true;
        [SerializeField] private float weight = 0f;
        [SerializeField] private Vector2 valueRange = new Vector2(0f, 100f);

        [HideInInspector] public int evaluatedContextVariableId = 0;

        public string evaluatedContextVariable = null;
        public CurveType responseCurveType = CurveType.Linear;

        public Dictionary<CurveType, ResponseCurve> curves = new Dictionary<CurveType, ResponseCurve>()
        {
            {
                CurveType.Linear, new LinearResponseCurve()
            },
            {
                CurveType.Quadratic, new QuadraticResponseCurve()
            },

            {
                CurveType.Logistic, new LogisticResponseCurve()
            },
            {
                CurveType.Animation, new AnimationResponseCurve()
            }
        };

        /// <summary>
        /// Utility curve to evaluate current consideration
        /// </summary>
        public ResponseCurve UtilityCurve
        {
            get => curves[responseCurveType];
            set => curves[responseCurveType] = value;
        }

        public Consideration() {
            UtilityCurve = new LinearResponseCurve();
        }

        public float Evaluate(IAiContext context) {
            if (evaluatedContextVariable != null) {
                var paramValue = context.GetParameter(evaluatedContextVariable);
                var rangedValue = (paramValue - valueRange.x) / (valueRange.y - valueRange.x);
                var utility = UtilityCurve.EvaluateAt(Mathf.Clamp(rangedValue, 0f, 1f));
                return utility;
            }

            return 0;
        }
    }
}