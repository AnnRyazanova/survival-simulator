using System.Collections.Generic;
using UtilityAI_Base.Actions;
using UtilityAI_Base.Actions.Base;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.Contexts.Interfaces;

namespace UtilityAI_Base.Selectors
{
    public abstract class ActionSelector
    {
        public abstract UtilityPick Select(AiContext context, List<AbstractUtilityAction> actions);
    }
}