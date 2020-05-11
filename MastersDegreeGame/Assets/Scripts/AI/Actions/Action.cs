using System;
using System.Collections.Generic;
using AI.Actions.Interfaces;
using AI.Considerations.Interfaces;
using AI.Contexts.Interfaces;
using UnityEngine;

namespace AI.Actions
{
    /// <summary>
    /// Action base class
    /// All actions should inherit this
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class Action<T> : IAction<T>
    {
        /// <summary>
        /// How much time should pass before action can be invoked again 
        /// </summary>
        public float cooldownTime = 0f;

        /// <summary>
        /// Action is being executed 
        /// </summary>
        private bool _inExecution = false;
        
        private readonly List<IConsideration<T>> _considerations = new List<IConsideration<T>>();

        public abstract float EvaluateAbsoluteUtility(IAiContext context);
        public abstract void Execute(IAiContext context);
        public abstract void Halt(float seconds);
        public abstract bool IsInExecution();
        public abstract bool CanBeInvoked();
    }
}