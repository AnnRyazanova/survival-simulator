using System;
using AI.Agents.Interfaces;
using AI.Contexts.Interfaces;

namespace AI.Contexts
{
    [Serializable]
    public class AiContext : IAiContext
    {
        public IAgent owner;
        public IAgent target;
    }
}