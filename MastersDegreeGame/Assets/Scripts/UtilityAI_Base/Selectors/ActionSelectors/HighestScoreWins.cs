using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityAI_Base.Actions;
using UtilityAI_Base.Actions.Base;
using UtilityAI_Base.Contexts;
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

        public override UtilityPick Select(AiContext context, List<AbstractUtilityAction> actions) {
            var maxUtility = 0f;
            UtilityPick highestScoreAction = null;
            foreach (var action in actions) {
                if (action != null) {
                    var utility = action.EvaluateAbsoluteUtility(context);
                    // Debug.Log(utility.UtilityAction.description + " " + utility.Score);
                    if (utility.Score > 0 && utility.Score >= maxUtility) {
                        maxUtility = utility.Score;
                        highestScoreAction = utility;
                    }
                }
            }
            
            return highestScoreAction;
        }
    }
}