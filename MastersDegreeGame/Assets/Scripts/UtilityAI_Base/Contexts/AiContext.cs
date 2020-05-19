using System;
using UtilityAI_Base.Agents.Interfaces;
using UtilityAI_Base.Contexts.Interfaces;
using UtilityAI_Base.CustomAttributes;

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

    [Serializable]
    [NpcContext("Spider")]
    public class SpiderContext : AiContext
    {
        public SpiderContext(IAgent owner) : base(owner) {
        }

        public float DistanceToTarget { get; set; }
        public float EnemiesNearby { get; set; }
        public float Energy { get; set; }
    }
    
    [Serializable]
    [NpcContext("Sheep")]
    public class SheepContext : AiContext
    {
        public SheepContext(IAgent owner) : base(owner) {
        }

        public float DistanceToTarget { get; set; }
        public float EnemiesNearby { get; set; }
    }
}