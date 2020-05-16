using System;
using UnityEditor;
using UnityEngine;
using UtilityAI_Base.ResponseCurves.Interfaces;

namespace UtilityAI_Base.ResponseCurves
{
    /// <summary>
    /// Abstract class for utility curve representation
    /// All custom utility curves must implement this class 
    /// </summary>
    [Serializable]
    public abstract class ResponseCurve
    {
        /// <summary>
        /// Curve slope (direction)
        /// </summary>
        public CurveParameter slope = new CurveParameter();

        /// <summary>
        /// Curve exponent (bend)
        /// </summary>
        public CurveParameter exponent = new CurveParameter();

        /// <summary>
        /// Curve Vertical starting point
        /// </summary>
        public CurveParameter verticalShift = new CurveParameter();

        /// <summary>
        /// Curve horizontal staring point
        /// </summary>
        public CurveParameter horizontalShift = new CurveParameter();
        
        public abstract float EvaluateAt(float parameter);
        public abstract float CurveFunction(float parameter);
    }
}