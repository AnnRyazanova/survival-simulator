using System;
using UtilityAI_Base.Selectors.ConsiderationQualifiers;

namespace UtilityAI_Base.Selectors.Factories
{
    public sealed class ConsiderationsQualifierFactory
    {
        private static readonly ProductQualifier Product = new ProductQualifier();
        private static readonly AverageQualifier Average = new AverageQualifier();
        
        public static ConsiderationsQualifier GetQualifier(QualifierType type) {
            switch (type) {
                case QualifierType.Product:
                    return Product;
                case QualifierType.Average:
                    return Average;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}