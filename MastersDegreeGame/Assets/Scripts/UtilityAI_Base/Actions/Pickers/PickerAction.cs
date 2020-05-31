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

        public List<InputConsideration> considerations = new List<InputConsideration>();

        public override UtilityPick EvaluateAbsoluteUtility(AiContext context) {
            // TODO: Maybe disable checks ?? or add some new =)
            if (evaluatedParamName != AiContextVariable.None) {
                List<float> averageScores = null;
                foreach (var inputConsideration in considerations) {
                    var scores = inputConsideration.Evaluate(context);
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

                return new UtilityPick(this, maxAvg, maxIdx);
            }

            return null;
        }

        public override void Execute(AiContext context, UtilityPick pick) {
            _lastInvokedTime = Time.time;
            _invokedTimes++;
            _inExecution = true;
            actionTask?.Invoke(context, pick);
        }
    }
}