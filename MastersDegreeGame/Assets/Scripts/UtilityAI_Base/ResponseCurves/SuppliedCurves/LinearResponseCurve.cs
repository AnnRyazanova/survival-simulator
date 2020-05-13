using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UtilityAI_Base.ResponseCurves.SuppliedCurves
{
    /// <summary>
    /// Linear-style response curve with float datatype
    /// Slope-intercept representation  y = m(x - c) + b  
    /// </summary>
    /// 
    [Serializable]
    public class LinearResponseCurve : ResponseCurve
    {
        #region public properties

        /// <summary>
        /// Curve slope (direction)
        /// </summary>
        public float slope;

        /// <summary>
        /// Curve exponent (bend)
        /// </summary>
        public float exponent;

        /// <summary>
        /// Curve Vertical starting point
        /// </summary>
        public float verticalShift;

        /// <summary>
        /// Curve horizontal staring point
        /// </summary>
        public float horizontalShift;

        #endregion

        #region constructors

        /// <summary>
        /// Default preset constructor
        /// </summary>
        public LinearResponseCurve() {
            slope = 1f;
            verticalShift = 1f;
            horizontalShift = 1f;
            exponent = 1f;
        }

        public LinearResponseCurve(float slope, float exponent, float verticalShift, float horizontalShift) {
            this.slope = slope;
            this.exponent = exponent;
            this.verticalShift = verticalShift;
            this.horizontalShift = horizontalShift;
        }

        #endregion

        #region overloaded interface methods

        public override float EvaluateAt(float parameter) => CurveFunction(parameter);

        public override float CurveFunction(float parameter) => slope * (parameter - horizontalShift) + verticalShift;

        #endregion
    }
}