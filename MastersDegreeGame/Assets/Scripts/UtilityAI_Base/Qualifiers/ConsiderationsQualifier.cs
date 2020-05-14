using System;
using System.Collections.Generic;
using UtilityAI_Base.Considerations;

namespace UtilityAI_Base.Qualifiers
{
    [Serializable]
    public abstract class ConsiderationsQualifier
    {
        public string description = "qualifier";
        public abstract float Qualify(List<Consideration> considerations);
    }
    [Serializable]
    public class ProductQualifier : ConsiderationsQualifier
    {
        public new string description = "product qualifier";
        public override float Qualify(List<Consideration> considerations) {
            throw new NotImplementedException();
        }
    }
}