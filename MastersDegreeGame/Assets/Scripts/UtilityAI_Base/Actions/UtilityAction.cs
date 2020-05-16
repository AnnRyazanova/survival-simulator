using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.Contexts.Interfaces;
using UtilityAI_Base.CustomAttributes;
using UtilityAI_Base.Selectors;

namespace UtilityAI_Base.Actions
{
    /// <summary>
    /// Action base class
    /// All actions should inherit this
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "New Action", menuName = "UtilityAI/Empty Action")]
    public class UtilityAction : ScriptableObject
    {
        /// <summary>
        /// How much time should pass before action can be invoked again 
        /// </summary>
        public float cooldownTime = 0f;
        
        /// <summary>
        /// Action description
        /// </summary>
        public string description = "Action";

        /// <summary>
        /// Action weight to be applied after Utility calculation. Adjusting weights make this action
        /// Less/more probable to be executed 
        /// </summary>
        public float actionWeight = 1f;
        
        /// <summary>
        /// Action is being executed 
        /// </summary>
        private bool _inExecution = false;
        public ConsiderationsQualifier qualifier = new ProductQualifier();
        public List<Consideration> considerations = new List<Consideration>();

        /// <summary>
        ///  Evaluate absolute (raw) utility score of performing action from Considerations utilities
        /// </summary>
        /// <param name="context">AI Context (game world state)</param>
        /// <returns>Absolute (raw) utility score of performing this action</returns>
        public virtual float EvaluateAbsoluteUtility(IAiContext context) {
            return qualifier.Qualify(context, considerations);
        }

        public void Awake() {
            considerations = new List<Consideration>();
        }

        /// <summary>
        /// Execute current action in current context
        /// </summary>
        /// <param name="context">AI Context (game world state)</param>
        public virtual void Execute(IAiContext context) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Halt executing for specified seconds 
        /// </summary>
        /// <param name="seconds"> seconds to halt execution for</param>
        public virtual void Halt(float seconds) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check if current action is already in execution
        /// </summary>
        /// <returns>if action is being executed</returns>
        public virtual bool IsInExecution() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check if action can be invoked
        /// </summary>
        /// <returns>If action can be invoked</returns>
        public virtual bool CanBeInvoked() {
            throw new NotImplementedException();
        }
    }
}