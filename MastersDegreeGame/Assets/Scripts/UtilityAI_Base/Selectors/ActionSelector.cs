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
        public string Name;
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

    [Serializable]
    [ActionSelectorAttribute("Dual Reasoner")]
    public sealed class DualUtilityReasoner : ActionSelector
    {
        private class UtilityWeights
        {
            public float Weight = 0f;
            public float Rank = 0f;
            public UtilityAction UAction = null;
        }
        
        public override UtilityAction Select(IAiContext context, List<UtilityAction> actions) {
            var utilities = new UtilityWeights[actions.Count];
            var cumulativeWeight = 0f;
            for (var i = 0; i < actions.Count; i++) {
                utilities[i].Weight = actions[i].EvaluateAbsoluteUtility(context);
                utilities[i].UAction = actions[i];
                cumulativeWeight += utilities[i].Weight;
            }

            foreach (var utilityWeight in utilities) {
                utilityWeight.Rank = utilityWeight.Weight / cumulativeWeight;
            }

            return null;
        }
    }
}