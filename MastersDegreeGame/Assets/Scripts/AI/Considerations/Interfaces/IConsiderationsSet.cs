using AI.Contexts.Interfaces;

namespace AI.Considerations.Interfaces
{
    public interface IConsiderationsSet
    {
        float EvaluateAll(IAiContext context);
    }
}