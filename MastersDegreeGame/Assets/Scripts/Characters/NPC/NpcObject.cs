using System;
using Characters.Animations;
using Characters.Systems.Combat;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.NPC
{
    public class NpcObject : Object, ICombatTarget
    {
        public HealthProperty Health { get; private set; }
        public CharacterAnimatorController animatorController;

        protected override void Start()
        {
            base.Start();
            Health = GetComponent<HealthProperty>();

            type = ObjectType.Mob;
        }

        public void TakeDamage(ICombatAggressor aggressor) {
            Health.RemovePoints(aggressor.GetDamage().value);
            if (Health.CurrentPoints == 0) {
                animatorController.OnDie();
                transform.GetChild(0).GetComponent<MeshCollider>().enabled = false;
                GetComponent<NavMeshAgent>().enabled = false;
            }
        }
    }
}
