using System.Collections;
using System.Collections.Generic;
using Characters.Animations;
using Characters.NPC;
using Characters.Systems.Combat;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Player
{
    public class PlayerObject : Object, ICombatAggressor, ICombatTarget
    {
        [SerializeField] private float hitDelaySeconds;
        
        public CharacterAnimatorController animatorController;
        private CapsuleCollider _capsuleCollider;

        public HealthProperty Health { get; private set; }
        public HungerProperty Hunger { get; private set; }
        public DamageProperty Damage { get; private set; }
        public WarmProperty Warm { get; private set; }
        public EnergyProperty Energy { get; private set; }

        protected override void Start()
        {
            _capsuleCollider = GetComponent<CapsuleCollider>();
            base.Start();
            type = ObjectType.Player;
            Health = GetComponent<HealthProperty>();
            Hunger = GetComponent<HungerProperty>();
            Damage = GetComponent<DamageProperty>();
            Warm = GetComponent<WarmProperty>();
            Energy = GetComponent<EnergyProperty>();
        }

        public void AttackTarget(ICombatTarget target) {
            StartCoroutine(DoDamage(target, hitDelaySeconds));
        }
        
        public DamageProperty GetDamage() => Damage;

        public void TakeDamage(ICombatAggressor aggressor) {
            animatorController.OnTakeDamage();
            StartCoroutine(DoTakeDamage(aggressor));
        }
        
        public IEnumerator DoDamage(ICombatTarget target, float delay) {
            yield return new WaitForSecondsRealtime(delay);
            target.TakeDamage(this);
        }
        
        public IEnumerator DoTakeDamage(ICombatAggressor aggressor, float delay = 0) {
            yield return new WaitForSecondsRealtime(delay);
            Health.RemovePoints(aggressor.GetDamage().value);
            if (Health.CurrentPoints == 0 && _capsuleCollider != null) {
                animatorController.OnDie();
                _capsuleCollider.enabled = false;
            }
        }
    }
}
