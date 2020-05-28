using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.Contexts.Interfaces;

namespace UtilityAI_Base.Actions
{
    public class EvaluationResult
    {
        public float Utility { get; set; }
        public int TargetId { get; set; }

        public EvaluationResult(float utility, int targetId) {
            Utility = utility;
            TargetId = targetId;
        }
    }
    
    [Serializable]
    [CreateAssetMenu(fileName = "New Picker Action", menuName = "UtilityAI/Empty Picker Action")]
    public class UtilityActionWithParams : AbstractUtilityAction
    {
        [HideInInspector] public int evaluatedContextVariableId = 0;
        public string evaluatedContextVariable = null;

        public EvaluationResult GetBestChoice(IAiContext context) {
            var sequence = context.GetSequenceParameter<float>(evaluatedContextVariable);
            var evalResult = new EvaluationResult(0f, -1);
            var considerationsCount = considerations.Count;
            var seq = sequence as float[] ?? sequence.ToArray();
            for (var i = 0; i < seq.Length; ++i)  {
                var utility = 0f;
                foreach (var c in considerations) {
                    var evaluation = c.Evaluate(seq[i]);
                    if (Mathf.Abs(evaluation) < 1e-4 && c.canApplyVeto) return null;
                    utility += evaluation;
                }

                utility /= considerationsCount;
                if (utility >= evalResult.Utility) {
                    evalResult.Utility = utility;
                    evalResult.TargetId = i;
                }
            }
            return evalResult.TargetId == -1 ? null : evalResult;
        }

        public  float EvaluateAbsoluteUtility(IAiContext context) {
            throw new NotImplementedException();
        }

        public  void Execute(IAiContext context) {
            throw new System.NotImplementedException();
        }

        public override float EvaluateAbsoluteUtility(AiContext context) {
            throw new NotImplementedException();
        }

        public override void Execute(AiContext context) {
            throw new NotImplementedException();
        }
    }
}
