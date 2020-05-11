using System;
using AI.Agents.Interfaces;
using AI.Contexts.Interfaces;

namespace AI.Contexts
{
    [Serializable]
    public abstract class AiContext : IAiContext
    {
        public IAgent owner;
    }
}