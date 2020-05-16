using UnityEngine;

namespace UtilityAI_Base.ResponseCurves.SuppliedCurves
{
    /// <summary>
    /// Logistic sigmoid-style response curve with float datatype
    /// Slope-intercept representation  y = k * 1/(1 + e^-(mx+c)) + b   
    /// </summary>
    public class LogisticResponseCurve : ResponseCurve
    {
        #region public properties

        #endregion

        #region constructors

        public LogisticResponseCurve() {
            slope = 20f;
            exponent = .5f;
            verticalShift = 1f;
            horizontalShift = 0f;
        }

        #endregion

        #region overloaded interface methods

        public override float EvaluateAt(float parameter) => CurveFunction(parameter);

        public override float CurveFunction(float parameter) {
            var denominator = 1f + Mathf.Exp(-slope * (parameter - exponent));
            return Mathf.Clamp(verticalShift / denominator + horizontalShift, 0f, 1f);
        }

        #endregion
    }
}