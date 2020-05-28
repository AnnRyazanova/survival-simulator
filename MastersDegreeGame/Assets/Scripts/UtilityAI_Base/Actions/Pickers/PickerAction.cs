using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityAI_Base.Actions.Base;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.Contexts.Interfaces;

namespace UtilityAI_Base.Actions.Pickers
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Picker", menuName = "UtilityAI/Picker")]
    public class PickerAction : AbstractUtilityAction
    {
        public string evaluatedParamName = null;

        public List<InputConsideration> considerations;

        public void Awake() {
            considerations = new List<InputConsideration>();
        }
        
        public override float EvaluateAbsoluteUtility(AiContext context) {
            throw new NotImplementedException();
        }

        public override void Execute(AiContext context, UtilityPick pick) {
            throw new NotImplementedException();
        }
    }
}