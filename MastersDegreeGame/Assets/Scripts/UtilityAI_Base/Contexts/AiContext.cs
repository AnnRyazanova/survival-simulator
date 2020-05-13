using System;
using UtilityAI_Base.Agents.Interfaces;
using UtilityAI_Base.Contexts.Interfaces;

namespace UtilityAI_Base.Contexts
{
    [Serializable]
    public class AiContext : IAiContext
    {
        public IAgent owner;

        public AiContext(IAgent owner) {
            this.owner = owner;
        }
    }
}