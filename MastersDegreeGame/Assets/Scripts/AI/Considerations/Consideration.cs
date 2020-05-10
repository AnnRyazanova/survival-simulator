using AI.Considerations.Interfaces;
using AI.Contexts.Interfaces;
using AI.ResponseCurves.Interfaces;

namespace AI.Considerations
{
    /// <summary>
    /// Abstract Consideration implementation
    /// All considerations should implement this class instead of an interface 
    /// </summary>
    public abstract class Consideration: IConsideration
    {
        /// <summary>
        /// Utility curve to evaluate current consideration
        /// </summary>
        public IResponseCurve<float> UtilityCurve { get; }

        public Consideration(IResponseCurve<float> utilityCurve) {
            UtilityCurve = utilityCurve;
        }
        
        public virtual float Evaluate(IAiContext context) {
            throw new System.NotImplementedException();
        }
    }
}