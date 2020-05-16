using UnityEngine;

namespace UtilityAI_Base.ResponseCurves.SuppliedCurves
{
    /// <summary>
    /// Exponential-style response curve
    /// Slope-intercept representation  y = m(x - c) ^ k + b  
    /// </summary>
    public class QuadraticResponseCurve : ResponseCurve
    {
        #region public properties

        #endregion

        #region constructors

        public QuadraticResponseCurve() {
            slope = 1f;
            exponent = 2f;
            horizontalShift = 0f;
            verticalShift = 0f;
        }

        #endregion

        #region overloaded interface methods

        public override float EvaluateAt(float parameter) => CurveFunction(parameter);

        public override float CurveFunction(float parameter) =>
            slope * Mathf.Pow(parameter - horizontalShift, exponent) + verticalShift;

        #endregion
    }
}