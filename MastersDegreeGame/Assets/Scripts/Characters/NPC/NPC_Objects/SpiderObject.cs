using System.Collections;
using Characters.Animations;
using Characters.Systems.Combat;
using UnityEngine;

namespace Characters.NPC.NPC_Objects
{
    public class SpiderObject : NpcObject, ICombatAggressor
    {
        public DamageProperty Damage { get; private set; }
        
        protected override void Start() {
            base.Start();
            Damage = GetComponent<DamageProperty>();
        }

        public void AttackTarget(ICombatTarget target) {
            Debug.Log("Attack");
            animatorController.OnAttackMelee();
            StartCoroutine(DoDamage(target, 0.1f));
        }

        public IEnumerator DoDamage(ICombatTarget target, float delay) {
            yield return new WaitForSecondsRealtime(delay);
            target.TakeDamage(this);
        }

        public DamageProperty GetDamage() => Damage;
    }
}