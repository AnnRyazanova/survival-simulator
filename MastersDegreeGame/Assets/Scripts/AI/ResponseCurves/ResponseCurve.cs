using AI.ResponseCurves.Interfaces;

namespace AI.ResponseCurves
{
    /// <summary>
    /// Abstract class for utility curve representation
    /// All custom utility curves must implement this class 
    /// </summary>
    /// <typeparam name="T">curve axis data type</typeparam>
    public abstract class ResponseCurve<T>: IResponseCurve<T>
    {
        public abstract T EvaluateAt(T parameter);
        public abstract T CurveFunction(T parameter);
    }
}