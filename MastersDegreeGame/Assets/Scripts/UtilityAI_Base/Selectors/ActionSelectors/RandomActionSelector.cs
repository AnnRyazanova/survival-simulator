using System.Collections.Generic;
using UtilityAI_Base.Actions;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.Contexts.Interfaces;
using Random = System.Random;

namespace UtilityAI_Base.Selectors.ActionSelectors
{
    public sealed  class RandomActionSelector : ActionSelector
    {
        private readonly Random _engine = new Random(42);
        public override UtilityAction Select(AiContext context, List<UtilityAction> actions) {
            return actions[_engine.Next(0, actions.Count)];
        }
    }
}