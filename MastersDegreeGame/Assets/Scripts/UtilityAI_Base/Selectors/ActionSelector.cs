using System;
using System.Collections.Generic;
using UtilityAI_Base.Actions;
using UtilityAI_Base.Contexts.Interfaces;
using UtilityAI_Base.CustomAttributes;

namespace UtilityAI_Base.Selectors
{
    public abstract class ActionSelector
    {
        public abstract UtilityAction Select(IAiContext context, List<UtilityAction> actions);
    }

    /// <summary>
    /// Highest scored action wins and shall be executed
    /// </summary>
    [Serializable]
    [ActionSelectorAttribute("Highest Score Wins")]
    public sealed class HighestScoreWins : ActionSelector
    {
        public override UtilityAction Select(IAiContext context, List<UtilityAction> actions) {
            var maxUtility = 0f;
            UtilityAction highestScoreAction = null;
            foreach (var action in actions) {
                if (action.CanBeInvoked()) {
                    var utility = action.EvaluateAbsoluteUtility(context);
                    if (utility >= maxUtility) {
                        maxUtility = utility;
                        highestScoreAction = action;
                    }
                }
            }

            return highestScoreAction;
        }
    }
}