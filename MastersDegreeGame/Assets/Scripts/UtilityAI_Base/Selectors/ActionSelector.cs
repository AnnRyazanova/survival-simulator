using System.Collections.Generic;
using UtilityAI_Base.Actions;
using UtilityAI_Base.Contexts.Interfaces;

namespace UtilityAI_Base.Selectors
{
    public abstract class ActionSelector
    {
        public abstract UtilityAction Select(IAiContext context, List<UtilityAction> actions);
    }
}