using UtilityAI_Base.ResponseCurves.Interfaces;

namespace UtilityAI_Base.ResponseCurves
{
    /// <summary>
    /// Abstract class for utility curve representation
    /// All custom utility curves must implement this class 
    /// </summary>
    /// <typeparam name="T">curve axis data type</typeparam>
    public abstract class ResponseCurve : IResponseCurve
    {
        public abstract float EvaluateAt(float parameter);
        public abstract float CurveFunction(float parameter);
    }
}