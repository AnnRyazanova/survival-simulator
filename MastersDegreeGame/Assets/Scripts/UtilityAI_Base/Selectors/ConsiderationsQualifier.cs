using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.Contexts.Interfaces;
using UtilityAI_Base.CustomAttributes;

namespace UtilityAI_Base.Selectors
{
    public enum QualifierType
    {
        Product,
        Sum,
        Average
    }
    
    [Serializable]
    public abstract class ConsiderationsQualifier
    {
        public string description = "qualifier";
        public abstract float Qualify(IAiContext context, List<Consideration> considerations);
    }
    
    /**
     * CUSTOM CONSIDERATION QUALIFIERS
     */

    [Serializable]
    public class ProductQualifier : ConsiderationsQualifier
    {
        public new string description = "product qualifier";
        
        public override float Qualify(IAiContext context, List<Consideration> considerations) {
            var product = considerations.Aggregate(1f, (current, consideration) =>
                current * consideration.Evaluate(context));
            var modificationFactor = 1f - 1f / considerations.Count;
            var makeUpValue = (1f - product) * modificationFactor;
            return product + makeUpValue * product;
        }
    }
    
    [Serializable]
    public class SumQualifier : ConsiderationsQualifier
    {
        public new string description = "sum qualifier";
        
        public override float Qualify(IAiContext context, List<Consideration> considerations) {
            return considerations.Aggregate(1f, (current, consideration) =>
                current + consideration.Evaluate(context));
        }
    }
    
    [Serializable]
    public class AverageQualifier : ConsiderationsQualifier
    {
        public new string description = "avg qualifier";
        
        public override float Qualify(IAiContext context, List<Consideration> considerations) {
            return considerations.Average(consideration => consideration.Evaluate(context));
        }
    }
}