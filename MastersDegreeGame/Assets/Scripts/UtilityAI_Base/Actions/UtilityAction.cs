using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.Contexts.Interfaces;

namespace UtilityAI_Base.Actions
{
    /// <summary>
    /// Action base class
    /// All actions should inherit this
    /// </summary>
    [Serializable]
    public abstract class UtilityAction
    {
        /// <summary>
        /// How much time should pass before action can be invoked again 
        /// </summary>
        public float cooldownTime = 0f;
        
        /// <summary>
        /// Action is being executed 
        /// </summary>
        private bool _inExecution = false;

        public List<Consideration> considerations = new List<Consideration>();
        
        /// <summary>
        ///  Evaluate absolute (raw) utility score of performing action from Considerations utilities
        /// </summary>
        /// <param name="context">AI Context (game world state)</param>
        /// <returns>Absolute (raw) utility score of performing this action</returns>
        public abstract float EvaluateAbsoluteUtility(IAiContext context);
        
        /// <summary>
        /// Execute current action in current context
        /// </summary>
        /// <param name="context">AI Context (game world state)</param>
        public abstract void Execute(IAiContext context);
        
        /// <summary>
        /// Halt executing for specified seconds 
        /// </summary>
        /// <param name="seconds"> seconds to halt execution for</param>
        public abstract void Halt(float seconds);
        
        /// <summary>
        /// Check if current action is already in execution
        /// </summary>
        /// <returns>if action is being executed</returns>
        public abstract bool IsInExecution();
        
        /// <summary>
        /// Check if action can be invoked
        /// </summary>
        /// <returns>If action can be invoked</returns>
        public abstract bool CanBeInvoked();
    }
}