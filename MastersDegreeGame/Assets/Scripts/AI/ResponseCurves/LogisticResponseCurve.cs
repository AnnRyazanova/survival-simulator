using AI.ResponseCurves.Interfaces;

namespace AI.ResponseCurves
{
    /// <summary>
    /// Logistic-style response curve
    /// </summary>
    /// <typeparam name="T">curve axis type</typeparam>
    public class LogisticResponseCurve<T>: ResponseCurve<T>
    {
        public override void EvaluateAt(T parameter) {
            throw new System.NotImplementedException();
        }
    }
}