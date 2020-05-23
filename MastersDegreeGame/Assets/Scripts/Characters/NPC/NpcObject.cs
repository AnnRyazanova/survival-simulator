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
            animatorController.OnTakeDamage();
            Health.RemovePoints(aggressor.GetDamage().value);
            if (Health.CurrentPoints == 0) {
                animatorController.OnDie();
                var parent = transform.parent;
                parent.GetComponent<NpcMainScript>().enabled = false;
                parent.GetComponent<NavMeshAgent>().enabled = false;
                parent.GetChild(0).GetComponent<MeshCollider>().enabled = false;
            }
        }
    }
}
