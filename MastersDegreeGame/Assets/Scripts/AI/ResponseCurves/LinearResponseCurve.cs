using AI.ResponseCurves.Interfaces;

namespace AI.ResponseCurves
{
    /// <summary>
    /// Linear-style response curve
    /// </summary>
    /// <typeparam name="T">curve axis type</typeparam>
    public class LinearResponseCurve<T>: ResponseCurve<T>
    {
        public override void EvaluateAt(T parameter) {
            throw new System.NotImplementedException();
        }
    }
}