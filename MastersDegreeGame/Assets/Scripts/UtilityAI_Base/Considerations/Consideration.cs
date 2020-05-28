using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityAI_Base.Contexts;
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
    public sealed class Consideration
    {
        [SerializeField] public string description = "n";
        [SerializeField] private float weight = 1f;
        [SerializeField] private Vector2 valueRange = new Vector2(0f, 100f);

        [HideInInspector] public int evaluatedContextVariableId = 0;

        public bool canApplyVeto = false;
        public bool isEnabled = true;

        public string evaluatedContextVariable = null;

        public AnimationCurve utilityCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

        public float ApplyCurveAt(float point) {
            return Mathf.Clamp(utilityCurve.Evaluate(point), 0f, 1f);
        }

        public Consideration(string description) => this.description = description;

        public Consideration() {
            
        }
        
        public float Evaluate(float value) {
            var rangedValue = (value - valueRange.x) / (valueRange.y - valueRange.x);
            return ApplyCurveAt(Mathf.Clamp(rangedValue, 0f, 1f));
        }
        
        public float Evaluate(AiContext context) {
            if (evaluatedContextVariable != null) {
                var paramValue = (float) context[evaluatedContextVariable];//context.GetParameter(evaluatedContextVariable);
                var rangedValue = (paramValue - valueRange.x) / (valueRange.y - valueRange.x);
                var utility = ApplyCurveAt(Mathf.Clamp(rangedValue, 0f, 1f));

                return utility;
            }

            return 0f;
        }
    }
}