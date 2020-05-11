namespace AI.ResponseCurves.SuppliedCurves
{
    /// <summary>
    /// Linear-style response curve with float datatype
    /// Slope-intercept representation  y = m(x - c) + b  
    /// </summary>
    public class LinearResponseCurve : ResponseCurve
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

        #region constructors

        /// <summary>
        /// Default preset constructor
        /// </summary>
        public LinearResponseCurve() {
            Slope = 1f;
            VerticalShift = 1f;
            HorizontalShift = 1f;
            Exponent = 1f;
        }

        public LinearResponseCurve(float slope, float exponent, float verticalShift, float horizontalShift) {
            Slope = slope;
            Exponent = exponent;
            VerticalShift = verticalShift;
            HorizontalShift = horizontalShift;
        }

        #endregion

        #region overloaded interface methods

        public override float EvaluateAt(float parameter) => CurveFunction(parameter);

        public override float CurveFunction(float parameter) => Slope * (parameter - HorizontalShift) + VerticalShift;

        #endregion
    }
}