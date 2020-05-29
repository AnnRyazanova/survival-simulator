using UtilityAI_Base.Actions;
using UtilityAI_Base.Actions.Base;

namespace UtilityAI_Base
{
    public enum UtilityActionType
    {
        Picker,
        Atomic
    }
    
    public sealed class UtilityPick
    {
        public AbstractUtilityAction UtilityAction = null;
        public float Score = 0f;
        public int SelectorIdx;
        public UtilityActionType ActionType;
        
        public UtilityPick(AbstractUtilityAction action, float score, int selectorIdx = -1) {
            UtilityAction = action;
            Score = score;
            SelectorIdx = selectorIdx;
            if (action != null) {
                ActionType = action is AtomicUtilityAction ? UtilityActionType.Atomic : UtilityActionType.Picker;
            }
        }
    }
}