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
        /// <summary>
        /// Curve slope (direction)
        /// </summary>
        public float Slope { get; set; }
        /// <summary>
        /// Curve exponent (bend)
        /// </summary>
        public float Exponent { get; set; }
        /// <summary>
        /// Curve Vertical starting point
        /// </summary>
        public float VerticalShift { get; set; }
        /// <summary>
        /// Curve horizontal staring point
        /// </summary>
        public float HorizontalShift { get; set; }
        
        public abstract void EvaluateAt(T parameter);
    }
}