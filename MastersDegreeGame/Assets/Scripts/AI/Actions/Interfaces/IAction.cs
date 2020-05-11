using AI.Contexts;
using AI.Contexts.Interfaces;

namespace AI.Actions.Interfaces
{
    /// <summary>
    /// Primitive Action representation
    /// </summary>
    public interface IAction<T>
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

        /// <summary>
        /// Check if current action is already in execution
        /// </summary>
        /// <returns>if action is being executed</returns>
        bool IsInExecution();
        /// <summary>
        /// Check if action can be invoked
        /// </summary>
        /// <returns>If action can be invoked</returns>
        bool CanBeInvoked();
    }
}
