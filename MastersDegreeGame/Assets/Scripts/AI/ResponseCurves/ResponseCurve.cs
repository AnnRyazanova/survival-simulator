using AI.ResponseCurves.Interfaces;

namespace AI.ResponseCurves
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