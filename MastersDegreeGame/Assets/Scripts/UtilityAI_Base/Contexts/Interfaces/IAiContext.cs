using System.Collections;
using System.Collections.Generic;

namespace UtilityAI_Base.Contexts.Interfaces
{
    /// <summary>
    /// world and object state representation
    /// </summary>
    public interface IAiContext
    {
        float GetParameter(string paramName);
        IEnumerable<T> GetSequenceParameter<T>(string paramName);
    }
}

