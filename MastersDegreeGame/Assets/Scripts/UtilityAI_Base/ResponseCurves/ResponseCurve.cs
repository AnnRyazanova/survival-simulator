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
    public abstract class ResponseCurve
    {
        public abstract float EvaluateAt(float parameter);
        public abstract float CurveFunction(float parameter);
    }
}