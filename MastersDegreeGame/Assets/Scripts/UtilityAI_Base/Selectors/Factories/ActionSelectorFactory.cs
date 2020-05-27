using System;
using UtilityAI_Base.Selectors.ActionSelectors;

namespace UtilityAI_Base.Selectors.Factories
{
    public enum ActionSelectorType
    {
        Random,
        DualUtility,
        HighestScoreWins
    }

    public sealed class ActionSelectorFactory
    {
        private readonly RandomActionSelector _randomActionSelector = new RandomActionSelector();
        private readonly DualUtilityReasoner _dualUtilityReasoner = new DualUtilityReasoner();
        private readonly HighestScoreWins _highestScoreWins = new HighestScoreWins();

        public ActionSelector GetSelector(ActionSelectorType actionSelectorType) {
            switch (actionSelectorType) {
                case ActionSelectorType.Random:
                    return _randomActionSelector;
                case ActionSelectorType.DualUtility:
                    return _dualUtilityReasoner;
                case ActionSelectorType.HighestScoreWins:
                    return _highestScoreWins;
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionSelectorType), actionSelectorType, null);
            }
        }
    }
}