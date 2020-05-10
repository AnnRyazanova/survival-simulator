using AI.Contexts;
using AI.Contexts.Interfaces;

namespace AI.Actions.Interfaces
{
    /// <summary>
    /// Primitive Action representation
    /// </summary>
    public interface IAction
    {
        /// <summary>
        ///  Evaluate absolute (raw) utility score of performing action from Considerations utilities
        /// </summary>
        /// <param name="context">AI Context (game world state)</param>
        /// <returns>Absolute (raw) utility score of performing this action</returns>
        float EvaluateAbsoluteUtility(IAiContext context);

        /// <summary>
        /// Execute current action in current context
        /// </summary>
        /// <param name="context">AI Context (game world state)</param>
        void Execute(IAiContext context);

        /// <summary>
        /// Halt executing for specified seconds 
        /// </summary>
        /// <param name="seconds"> seconds to halt execution for</param>
        void Halt(float seconds);
    }
}
