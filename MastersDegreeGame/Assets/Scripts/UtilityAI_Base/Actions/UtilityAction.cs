using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.Contexts.Interfaces;
using UtilityAI_Base.Selectors;
using UtilityAI_Base.Selectors.ConsiderationQualifiers;

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

        private float _utility = 0f;
        public float Utility => _utility;

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
        /// Callback for inertia coroutine
        /// Used to add weight to action, so it wont be disrupted unless found action with much higher priority
        /// </summary>
        /// <returns> WaitForSeconds coroutine object </returns>
        public IEnumerator AddInertia() {
            var oldWeight = _actionWeight;
            _actionWeight *= 2f;
            yield return new WaitForSeconds(_cooldownTime);
            _actionWeight = oldWeight;
        }

        /// <summary>
        /// Callback for cooldown coroutine
        /// Used to add "trace" to action, so it wont be called consecutively 
        /// </summary>
        /// <returns> WaitForSeconds coroutine object </returns>
        public IEnumerator SetInCooldown() {
            var oldWeight = _actionWeight;
            _actionWeight = 0f;
            yield return new WaitForSeconds(_cooldownTime);
            _actionWeight = oldWeight;
        }

        /// <summary>
        ///  Evaluate absolute (raw) utility score of performing action from Considerations utilities
        /// </summary>
        /// <param name="context">AI Context (game world state)</param>
        /// <returns>Absolute (raw) utility score of performing this action</returns>
        public virtual float EvaluateAbsoluteUtility(IAiContext context) {
            _utility = qualifier.Qualify(context, considerations);
            return _utility;
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