namespace Characters.Systems.Combat
{
    public interface ICombatTarget
    {
        void TakeDamage(ICombatAggressor aggressor);
    }
}
