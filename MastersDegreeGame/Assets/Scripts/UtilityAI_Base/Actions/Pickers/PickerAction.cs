using System.Collections.Generic;
using UnityEngine;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.Contexts.Interfaces;

namespace UtilityAI_Base.Actions.Pickers
{ 
    public sealed class Pick<T>
    {
        public float Utility { get; set; }
        public T Target { get; set; }

        public Pick(float utility, T target) {
            Utility = utility;
            Target = target;
        }
    }
    
    public abstract class PickerAction<T> : AbstractUtilityAction
    {
        public string evaluatedParamName = null;
        public abstract Pick<T> GetBest(AiContext context);
    }
}