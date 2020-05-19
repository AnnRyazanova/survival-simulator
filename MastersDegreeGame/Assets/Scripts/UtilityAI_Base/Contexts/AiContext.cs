using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UtilityAI_Base.Agents.Interfaces;
using UtilityAI_Base.Contexts.Interfaces;
using UtilityAI_Base.CustomAttributes;

namespace UtilityAI_Base.Contexts
{
    [Serializable]
    public class AiContext : IAiContext
    {
        public IAgent owner;
        protected Dictionary<string, float> PropertyValues = new Dictionary<string, float>();
        
        public AiContext(IAgent owner) {
            this.owner = owner;
        }

        public float GetParameter(string paramName) {
            return PropertyValues[paramName];
        }
        
        public virtual void Fill() {
            foreach (var property in this.GetType().GetProperties()) {
                PropertyValues.Add(property.Name, (float)property.GetValue(this));
            }
        }
    }

    [Serializable]
    [NpcContext("Spider")]
    public class SpiderContext : AiContext
    {
        public SpiderContext(IAgent owner) : base(owner) {
            DistanceToTarget = 0.5f;
            EnemiesNearby = 0.4f;
            Energy = 0f;
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
            DistanceToTarget = 0.3f;
        }

        public float DistanceToTarget { get; set; }
        public float EnemiesNearby { get; set; }
    }
}