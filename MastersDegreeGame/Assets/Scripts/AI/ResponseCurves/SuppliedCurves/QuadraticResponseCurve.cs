using UnityEngine;

namespace AI.ResponseCurves.SuppliedCurves
{
    /// <summary>
    /// Exponential-style response curve
    /// Slope-intercept representation  y = m(x - c) ^ k + b  
    /// </summary>
    public class QuadraticResponseCurve : ResponseCurve
    {
        #region public properties

        /// <summary>
        /// Curve slope (direction)
        /// </summary>
        public float Slope { get; set; }

        /// <summary>
        /// Curve exponent (bend)
        /// </summary>
        public float Exponent { get; set; }

        /// <summary>
        /// Curve Vertical starting point
        /// </summary>
        public float VerticalShift { get; set; }

        /// <summary>
        /// Curve horizontal staring point
        /// </summary>
        public float HorizontalShift { get; set; }

        #endregion

        #region constructora

        public QuadraticResponseCurve() {
            Slope = 1f;
            Exponent = 2f;
            HorizontalShift = 0f;
            VerticalShift = 0f;
        }

        #endregion

        #region overloaded interface methods

        public override float EvaluateAt(float parameter) => CurveFunction(parameter);

        public override float CurveFunction(float parameter) =>
            Slope * Mathf.Pow(parameter - HorizontalShift, Exponent) + VerticalShift;

        #endregion
    }
}