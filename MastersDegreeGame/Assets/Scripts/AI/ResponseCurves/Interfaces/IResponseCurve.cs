namespace AI.ResponseCurves.Interfaces
{
    /// <summary>
    /// Interface for a response curve used in calculating consideration utility score
    /// Utility is ALWAYS a float value between 0.0 and 1.0
    /// </summary>
    /// <typeparam name="T">axis data type</typeparam>
    public interface IResponseCurve<T>
    {
        /// <summary>
        /// Evaluate curve at point parameter
        /// </summary>
        /// <param name="parameter">point at which curve should be evaluated</param>
        float EvaluateAt(T parameter);

        /// <summary>
        /// Curve function representation (E.g. y = mx + b)
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        float CurveFunction(T parameter);
    }
}