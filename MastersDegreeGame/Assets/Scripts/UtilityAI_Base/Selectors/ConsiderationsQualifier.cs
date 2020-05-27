using System;
using System.Collections.Generic;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.Contexts.Interfaces;

namespace UtilityAI_Base.Selectors
{
    public enum QualifierType
    {
        Product,
        Average
    }
    
    [Serializable]
    public abstract class ConsiderationsQualifier
    {
        public string description = "qualifier";
        public abstract float Qualify(IAiContext context, List<Consideration> considerations);
    }
}