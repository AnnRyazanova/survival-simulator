using System.Collections.Generic;
using AI.Actions.Interfaces;
using AI.Considerations.Interfaces;
using AI.Contexts.Interfaces;

namespace AI.Actions
{
    public abstract class Action: IAction
    {
        private readonly List<IConsideration> _considerations = new List<IConsideration>();
        
        public abstract float EvaluateAbsoluteUtility(IAiContext context);
        public abstract void Execute(IAiContext context);
        public abstract void Halt(float seconds);
    }
}