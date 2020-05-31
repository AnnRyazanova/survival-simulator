using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityAI_Base.Actions.Pickers;
using UtilityAI_Base.Considerations.Interfaces;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.Contexts.Interfaces;

namespace UtilityAI_Base.Considerations
{
    [Serializable]
    public class InputConsideration : Consideration
    {
        public List<float> Evaluate(AiContext context) {
            var pick = new List<float>();
            if (evaluatedContextVariable != AiContextVariable.None) {
                if (context.GetParameter(evaluatedContextVariable) is List<float> evaluatedParams) {
                    foreach (var eval in evaluatedParams) {
                        pick.Add(EvaluateAt(eval));
                    }
                }
            }
            return pick;
        }
    }
}