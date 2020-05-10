namespace AI.ResponseCurves
{
    /// <summary>
    /// Exponential-style response curve
    /// </summary>
    /// <typeparam name="T">curve axis type</typeparam>
    public abstract class ExponentialResponseCurve<T>: ResponseCurve<T>
    {
        public override void EvaluateAt(T parameter) {
            throw new System.NotImplementedException();
        }
    }
}