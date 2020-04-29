using System.Collections;
using System.Collections.Generic;

namespace Characters.Systems.Combat
{
    public interface ICombatAggressor
    {
        void AttackTarget(ICombatTarget target);
        IEnumerator DoDamage(ICombatTarget player, float delay);
    }
}
