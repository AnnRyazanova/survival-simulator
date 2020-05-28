using UtilityAI_Base.Actions.Base;

namespace UtilityAI_Base
{
    public sealed class UtilityPick
    {
        public AbstractUtilityAction UtilityAction = null;
        public float UtilityScore = 0f;
        public int SelectorIdx = -1;
    }
}