using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityAI_Base.Actions;
using UtilityAI_Base.Contexts.Interfaces;
using UtilityAI_Base.CustomAttributes;

namespace UtilityAI_Base.Selectors.ActionSelectors
{
    /// <summary>
    /// Highest scored action wins and shall be executed
    /// </summary>
    [Serializable]
    [ActionSelector("Highest Score Wins")]
    public sealed class HighestScoreWins : ActionSelector
    {
        public string name;

        public override UtilityAction Select(IAiContext context, List<UtilityAction> actions) {
            var maxUtility = 0f;
            UtilityAction highestScoreAction = null;
            foreach (var action in actions) {
                var utility = action.EvaluateAbsoluteUtility(context);
                if (utility >= maxUtility) {
                    maxUtility = utility;
                    highestScoreAction = action;
                }
            }
            return highestScoreAction;
        }
    }
}