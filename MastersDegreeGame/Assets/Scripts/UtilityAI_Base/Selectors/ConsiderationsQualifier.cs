using System;
using System.Collections.Generic;
using System.Linq;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.Contexts.Interfaces;
using UtilityAI_Base.CustomAttributes;

namespace UtilityAI_Base.Selectors
{
    [Serializable]
    public abstract class ConsiderationsQualifier
    {
        public string description = "qualifier";
        public abstract float Qualify(IAiContext context, IEnumerable<Consideration> considerations);
    }
    
    /**
     * CUSTOM CONSIDERATION QUALIFIERS 
     */

    [Serializable]
    [ConsiderationsQualifier("product", typeof(ProductQualifier))]
    public class ProductQualifier : ConsiderationsQualifier
    {
        public new string description = "product qualifier";
        
        public override float Qualify(IAiContext context, IEnumerable<Consideration> considerations) {
            return considerations.Aggregate(1f, (current, consideration) =>
                current * consideration.Evaluate(context));
        }
    }
    
    [Serializable]
    [ConsiderationsQualifier("sum", typeof(SumQualifier))]
    public class SumQualifier : ConsiderationsQualifier
    {
        public new string description = "sum qualifier";
        
        public override float Qualify(IAiContext context, IEnumerable<Consideration> considerations) {
            return considerations.Aggregate(1f, (current, consideration) =>
                current * consideration.Evaluate(context));
        }
    }
}