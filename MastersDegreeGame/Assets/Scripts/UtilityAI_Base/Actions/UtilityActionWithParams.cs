using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilityAI_Base.Contexts.Interfaces;

namespace UtilityAI_Base.Actions
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Picker Action", menuName = "UtilityAI/Empty Picker Action")]
    public class UtilityActionWithParams : AbstractUtilityAction
    {
        [HideInInspector] public int evaluatedContextVariableId = 0;
        public string evaluatedContextVariable = null;

        public override float EvaluateAbsoluteUtility(IAiContext context) {
            return 0f;
        }

        public override void Execute(IAiContext context) {
            throw new System.NotImplementedException();
        }
    }
}
