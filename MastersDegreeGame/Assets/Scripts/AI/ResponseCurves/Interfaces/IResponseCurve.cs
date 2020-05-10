namespace AI.ResponseCurves.Interfaces
{
    /// <summary>
    /// Interface for a response curve used in calculating consideration utility score
    /// </summary>
    /// <typeparam name="T">axis data type</typeparam>
    public interface IResponseCurve<in T>
    {
        /// <summary>
        /// Evaluate curve at point parameter
        /// </summary>
        /// <param name="parameter">point at which curve should be evaluated</param>
        void EvaluateAt(T parameter);
    }
}
