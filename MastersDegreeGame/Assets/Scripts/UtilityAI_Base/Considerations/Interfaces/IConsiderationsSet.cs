using UtilityAI_Base.Contexts.Interfaces;

namespace UtilityAI_Base.Considerations.Interfaces
{
    public interface IConsiderationsSet
    {
        float EvaluateAll(IAiContext context);
    }
}