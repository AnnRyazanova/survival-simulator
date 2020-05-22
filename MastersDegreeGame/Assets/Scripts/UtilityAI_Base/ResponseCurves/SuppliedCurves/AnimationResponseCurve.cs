using System;
using UnityEngine;

namespace UtilityAI_Base.ResponseCurves.SuppliedCurves
{
    [Serializable]
    public class AnimationResponseCurve : ResponseCurve
    {
        public AnimationCurve curve;

        public AnimationResponseCurve() {
            curve = AnimationCurve.Linear(0f,0f,1f, 1f);
            responseCurveType = CurveType.Animation;
        }
        public override float EvaluateAt(float parameter) {
            return Mathf.Clamp(curve.Evaluate(parameter), 0f, 1f);
        }

        public override float CurveFunction(float parameter) {
            return curve.Evaluate(parameter);
        }

        public override void SetDefaults() {
            curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        }
    }
}