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

        
        public abstract float EvaluateAt(float parameter);
        public abstract float CurveFunction(float parameter);
    }
}