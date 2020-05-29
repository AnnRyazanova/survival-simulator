using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityAI_Base.Actions.Base;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.Contexts.Interfaces;

namespace UtilityAI_Base.Actions.Pickers
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Action w Inputs", menuName = "UtilityAI/Action w Inputs")]
    public class PickerAction : AbstractUtilityAction
    {
        public AiContextVariable evaluatedParamName = AiContextVariable.None;

        public List<InputConsideration> considerations;

        public void Awake() {
            considerations = new List<InputConsideration>();
        }

        public override UtilityPick EvaluateAbsoluteUtility(AiContext context) {
            // TODO: Maybe disable checks ?? 
            var c = 0;
            if (evaluatedParamName != AiContextVariable.None) {
                List<float> averageScores = null;
                foreach (var inputConsideration in considerations) {
                    var scores = inputConsideration.Evaluate(context);
                    c = scores.Count;
                    if(averageScores == null) averageScores = new List<float>(new float[scores.Count]);
                    for (var i = 0; i < scores.Count; i++) {
                        averageScores[i] += scores[i];
                    }
                }

                var considerationsCount = (float)considerations.Count;
                var maxIdx = 0;
                var maxAvg = 0f;
                if (averageScores != null)
                    for (var i = 0; i < averageScores.Count; i++) {
                        averageScores[i] /= considerationsCount;
                        if (averageScores[i] > maxAvg) {
                            maxAvg = averageScores[i];
                            maxIdx = i;
                        }
                    }

                // Debug.Log("FROM " + maxAvg + " " + maxIdx + " " + c);
                return new UtilityPick(this, maxAvg, maxIdx);
            }

            return null;
        }

        public override void Execute(AiContext context, UtilityPick pick) {
            Debug.Log(context.GetParameter(evaluatedParamName));
        }
    }
}