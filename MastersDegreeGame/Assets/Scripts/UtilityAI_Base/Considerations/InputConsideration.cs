using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityAI_Base.Considerations.Interfaces;
using UtilityAI_Base.Contexts;

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
                else {
                    if (context.GetParameter(evaluatedContextVariable) is float evaluatedParam) {
                        Debug.Log( "SSSSSSSSSSs " + evaluatedParam);
                    }
                }
            }
            return pick;
        }
    }
}