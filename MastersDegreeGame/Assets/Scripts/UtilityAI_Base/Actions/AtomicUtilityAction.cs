using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Serialization;
using UtilityAI_Base.Actions.Base;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.Considerations.Interfaces;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.Contexts.Interfaces;
using UtilityAI_Base.Selectors;
using UtilityAI_Base.Selectors.ConsiderationQualifiers;

namespace UtilityAI_Base.Actions
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Action", menuName = "UtilityAI/Empty Action")]
    public class AtomicUtilityAction : AbstractUtilityAction
    {
        /// <summary>
        /// List of all considerations needed to evaluate this actions' utility score 
        /// </summary>
        public List<ContextConsideration> considerations;

        public void Awake() {
            considerations = new List<ContextConsideration>();
        }

        public override float EvaluateAbsoluteUtility(AiContext context) {
            return _actionWeight * qualifier.Qualify(context, considerations);
        }
        
        public override void Execute(AiContext context, UtilityPick pick) {
            _lastInvokedTime = Time.time;
            _invokedTimes++;
            _inExecution = true;
            actionTask.Invoke(context, pick);
        }
    }
}