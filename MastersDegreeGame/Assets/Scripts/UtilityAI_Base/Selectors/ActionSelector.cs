using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilityAI_Base.Actions;
using UtilityAI_Base.Contexts.Interfaces;
using UtilityAI_Base.CustomAttributes;
using UtilityAI_Base.ExtensionMethods;
using Random = UnityEngine.Random;

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
                    var utility = action.ActionWeight * action.EvaluateAbsoluteUtility(context);
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
        public class UtilityWeights
        {
            public float Weight = 0f;
            public readonly float Rank = 0f;
            public readonly UtilityAction UAction = null;

            public UtilityWeights(float rank, UtilityAction action) {
                Rank = rank;
                UAction = action;
            }
        }

        public override UtilityAction Select(IAiContext context, List<UtilityAction> actions) {
            var utilities = new List<UtilityWeights>();
            foreach (var action in actions) {
                float utility = action.EvaluateAbsoluteUtility(context);
                if (utility > 0f) {
                    utilities.Add(new UtilityWeights(utility, action));
                }
            }
            
            var count = utilities.Count;
            if (count > 0) {
                utilities.Sort((first, second) => first.Rank.CompareTo(second));
                
                var std = utilities.GetStd();
                var sum = 0f;

                var minWeight = float.PositiveInfinity;
                var maxWeight = 0f;
                
                var selected = new List<UtilityWeights>();
                foreach (var u in utilities) {
                    if(u.Rank < std) continue;
                    selected.Add(u);
                    sum += u.Rank;
                }

                foreach (var u in selected) {
                    u.Weight = u.Rank / sum;
                    if (u.Weight > maxWeight) maxWeight = u.Weight;
                    if (u.Weight < minWeight) minWeight = u.Weight;
                }      

                var rand = Random.Range(minWeight, maxWeight);
                return utilities.Find(u => u.Weight >= rand).UAction;
            }

            return null;
        }
    }
}