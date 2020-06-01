using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilityAI_Base.Actions.Base;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.Contexts.Interfaces;
using UtilityAI_Base.Selectors;
using UtilityAI_Base.Selectors.ConsiderationQualifiers;

namespace UtilityAI_Base.Actions.Pickers
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Action w Inputs", menuName = "UtilityAI/Action w Inputs")]
    public class PickerAction : AbstractUtilityAction
    {
        public AiContextVariable evaluatedParamName = AiContextVariable.None;

        public List<InputConsideration> considerations = new List<InputConsideration>();

        private List<float> GetSumScores(AiContext context) {
            List<float> averageScores = null;
            var vetoIndices = new List<int>();
            var count = (context.GetParameter(evaluatedParamName) as IEnumerable).Cast<object>().Count();
            foreach (var inputConsideration in considerations) {
                if (!inputConsideration.isEnabled) continue;
                var scores = inputConsideration.Evaluate(context, count);
                if(averageScores == null) averageScores = new List<float>(new float[scores.Count]);
                for (var i = 0; i < scores.Count; i++) {
                    var score = Mathf.Round(scores[i] * 1e+3f) / 1e+3f;
                    if (score == 0 && inputConsideration.canApplyVeto) {
                        vetoIndices.Add(i);
                    }
                    averageScores[i] += score;
                }
            }

            if (averageScores != null) {
                foreach (var vetoIndex in vetoIndices) {
                    averageScores[vetoIndex] = 0f;
                }
            }
           
            return averageScores;
        }
        
        public override UtilityPick EvaluateAbsoluteUtility(AiContext context) {
            if (evaluatedParamName != AiContextVariable.None) {
                var averageScores = GetSumScores(context);
                if (averageScores != null) {
                    var considerationsCount = (float)considerations.Count;
                    var maxIdx = -1;
                    var maxAvg = 0f;
                    for (var i = 0; i < averageScores.Count; i++) {
                        averageScores[i] /= considerationsCount;
                        if (averageScores[i] > maxAvg) {
                            maxAvg = averageScores[i];
                            maxIdx = i;
                        }
                    }
                
                    return new UtilityPick(this, ActionWeight * maxAvg, maxIdx);
                }
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