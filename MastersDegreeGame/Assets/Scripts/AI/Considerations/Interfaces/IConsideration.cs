using AI.Contexts;
using AI.Contexts.Interfaces;

namespace AI.Considerations.Interfaces
{
    public interface IConsideration
    {
        /// <summary>
        /// Evaluate current consideration based on a response curve
        /// </summary>
        /// <param name="context">>AI Context (game world state)</param>
        /// <returns> Current consideration response curve evaluation at given context </returns>
        float Evaluate(IAiContext context);
    }
}
