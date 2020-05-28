using System.Collections.Generic;
using UnityEngine;
using UtilityAI_Base.Agents.Interfaces;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.Contexts.Interfaces;

namespace UtilityAI_Base.Actions.Pickers
{
    [CreateAssetMenu(fileName = "New Attack Target", menuName = "UtilityAI/Pickers/Attack Target", order = 0)]
    public class AttackTargetPicker : PickerAction<IAgent>
    {
        public Consideration distanceConsideration = new Consideration("Distance");
        public Consideration healthConsideration = new Consideration("Health"); 
        
        public override Pick<IAgent> GetBest(AiContext context) {
            var options = (IEnumerable<IAgent>)context[evaluatedParamName];
            var owner = context.owner;
            var pick = new Pick<IAgent>(0f, null);
            var current = 0f;
            foreach (var option in options) {
                current = distanceConsideration.Evaluate(Vector3.Distance(option.GetCurrentWorldPosition(),
                    owner.GetCurrentWorldPosition()));
                current += healthConsideration.Evaluate(Vector3.Distance(option.GetCurrentWorldPosition(),
                    owner.GetCurrentWorldPosition()));
                if (current >= pick.Utility) {
                    pick.Utility = current;
                    pick.Target = option;
                }
            }

            return pick;
        }

        public override float EvaluateAbsoluteUtility(AiContext context) {
            throw new System.NotImplementedException();
        }

        public override void Execute(AiContext context) {
            throw new System.NotImplementedException();
        }
    }
}