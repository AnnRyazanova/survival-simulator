using System;
using System.Collections.Generic;
using Characters.NPC;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.Contexts.Interfaces;
using UtilityAI_Base.Intellect;
using UtilityAI_Base.Selectors;

namespace UtilityAI_Base.Actions
{
    [Serializable]
    public class ActionTask : UnityEvent<IAiContext>
    {
        
    }
    
    /// <summary>
    /// Action base class
    /// All actions should inherit this
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "New Action", menuName = "UtilityAI/Empty Action")]
    public class UtilityAction : ScriptableObject
    {
        public ActionTask actionTask;
        
        /// <summary>
        /// How much time should pass before action can be invoked again 
        /// </summary>
        private float _cooldownTime = 0f;

        private float _lastInvokedTime = 0f;

        public float CooldownTime
        {
            get => _cooldownTime;
            set => _cooldownTime = Mathf.Clamp(value, 0f, 1e+10f);
        }

        /// <summary>
        /// Action weight to be applied after Utility calculation. Adjusting weights make this action
        /// Less/more probable to be executed 
        /// </summary>
        private float _actionWeight = 1f;

        public float ActionWeight
        {
            get => _actionWeight;
            set => _actionWeight = Mathf.Clamp(value, 0f, 100f);
        }

        /// <summary>
        /// Action description
        /// </summary>
        public string description = "New Action Description";

        /// <summary>
        /// Max number of times current action can be invoked in a row
        /// </summary>
        public int maxConsecutiveInvocations = 0;

        private int _invokedTimes = 0;

        /// <summary>
        /// Action is being executed 
        /// </summary>
        private bool _inExecution = false;

        /// <summary>
        /// Qualifier to calculate utility of ALL considerations for this action
        /// </summary>
        [FormerlySerializedAs("qualifierTypeType")]
        public QualifierType qualifierType;

        public ConsiderationsQualifier qualifier = new ProductQualifier();

        /// <summary>
        /// List of all considerations needed to evaluate this actions' utility score 
        /// </summary>
        public List<Consideration> considerations = new List<Consideration>();

        public void Awake() {
            considerations = new List<Consideration>();
        }

        /// <summary>
        ///  Evaluate absolute (raw) utility score of performing action from Considerations utilities
        /// </summary>
        /// <param name="context">AI Context (game world state)</param>
        /// <returns>Absolute (raw) utility score of performing this action</returns>
        public virtual float EvaluateAbsoluteUtility(IAiContext context) {
            return qualifier.Qualify(context, considerations);
        }

        /// <summary>
        /// Execute current action in current context
        /// </summary>
        /// <param name="context">AI Context (game world state)</param>
        public virtual void Execute(IAiContext context) {
            _lastInvokedTime = Time.time;
            _invokedTimes++;
            _inExecution = true;
            actionTask.Invoke(context);
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
        public virtual bool IsInExecution() => _inExecution;

        /// <summary>
        /// Check if action can be invoked
        /// </summary>
        /// <returns>If action can be invoked</returns>
        public virtual bool CanBeInvoked() => !_inExecution && CooldownTime >= _lastInvokedTime &&
                                              _invokedTimes <= maxConsecutiveInvocations;
    }
}