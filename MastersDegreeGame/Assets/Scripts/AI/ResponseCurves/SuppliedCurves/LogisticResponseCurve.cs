using UnityEngine;

namespace AI.ResponseCurves.SuppliedCurves
{
    /// <summary>
    /// Logistic sigmoid-style response curve with float datatype
    /// Slope-intercept representation  y = k * 1/(1 + e^-(mx+c)) + b   
    /// </summary>
    public class LogisticResponseCurve : ResponseCurve
    {
        #region public properties

        /// <summary>
        /// Curve steepness (bend)
        /// </summary>
        public float Steepness { get; set; }

        /// <summary>
        /// Curve top max value 
        /// </summary>
        public float MaxValue { get; set; }

        /// <summary>
        /// Curve midpoint
        /// </summary>
        public float Midpoint { get; set; }

        /// <summary>
        /// Curve horizontal staring point
        /// </summary>
        public float HorizontalShift { get; set; }

        #endregion

        #region constructors

        public LogisticResponseCurve() {
            Steepness = 20f;
            Midpoint = .5f;
            MaxValue = 1f;
            HorizontalShift = 0f;
        }

        #endregion

        #region overloaded interface methods

        public override float EvaluateAt(float parameter) => CurveFunction(parameter);

        public override float CurveFunction(float parameter) {
            var denominator = 1f + Mathf.Exp(-Steepness * (parameter - Midpoint));
            return MaxValue / denominator + HorizontalShift;
        }

        #endregion
    }
}