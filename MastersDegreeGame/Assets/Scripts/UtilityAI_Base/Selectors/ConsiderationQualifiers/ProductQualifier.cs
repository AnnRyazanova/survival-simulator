using System;
using System.Collections.Generic;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.Contexts.Interfaces;

namespace UtilityAI_Base.Selectors.ConsiderationQualifiers
{
    [Serializable]
    public class ProductQualifier : ConsiderationsQualifier
    {
        public new string description = "product qualifier";
        
        public override float Qualify(AiContext context, List<Consideration> considerations) {
            var product = 1f;
            foreach (var consideration in considerations) {
                if (consideration.isEnabled) product *= consideration.Evaluate(context);
            }
            var modificationFactor = 1f - 1f / considerations.Count;
            var makeUpValue = (1f - product) * modificationFactor;
            return product + makeUpValue * product;
        }
    }
}