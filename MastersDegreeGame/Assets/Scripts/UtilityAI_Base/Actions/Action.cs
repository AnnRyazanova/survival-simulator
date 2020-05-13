using System;
using System.Collections.Generic;
using UtilityAI_Base.Actions.Interfaces;
using UtilityAI_Base.Considerations.Interfaces;
using UtilityAI_Base.Contexts.Interfaces;

namespace UtilityAI_Base.Actions
{
    /// <summary>
    /// Action base class
    /// All actions should inherit this
    /// </summary>
    [Serializable]
    public abstract class Action : IAction
    {
        /// <summary>
        /// How much time should pass before action can be invoked again 
        /// </summary>
        public float cooldownTime = 0f;

        /// <summary>
        /// Action is being executed 
        /// </summary>
        private bool _inExecution = false;
        
        private readonly List<IConsideration> _considerations = new List<IConsideration>();

        public abstract float EvaluateAbsoluteUtility(IAiContext context);
        public abstract void Execute(IAiContext context);
        public abstract void Halt(float seconds);
        public abstract bool IsInExecution();
        public abstract bool CanBeInvoked();
    }
}