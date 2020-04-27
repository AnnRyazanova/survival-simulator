using System;
using System.Collections;
using Characters.Animations;
using Characters.Player;
using Characters.Systems.Combat;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Characters.NPC
{
    public class NpcMainScript : GameCharacter, ICombatTarget, ICombatAggressor
    {
        [FormerlySerializedAs("mobObject")] public NpcObject npcObject;
        
        public void Start() {
            AnimatorController = new NpcAnimatorController(GetComponent<Animator>());
            attackRate = 1f;
            GetComponent<NavMeshAgent>().updatePosition = false;
        }

        public void tmpAiControllerFunc() {
            var hits = Physics.OverlapSphere(actionSphere.transform.position, itemSearchRadius);
            if (hits != null && AnimatorController != null) {
                foreach (var hit in hits) {
                    var colliderParent = hit.gameObject.transform.parent;
                    if (colliderParent != null && colliderParent.GetComponent<PlayerMainScript>() != null) {
                        if (Time.time >= lastAttackTime) {
                            AttackTarget(colliderParent.GetComponent<PlayerMainScript>());
                            lastAttackTime = Time.time + 1f / attackRate;
                        }
                    }
                }
            }
        }

        private void Update() {
            tmpAiControllerFunc();
        }

        public void TakeDamage(DamageProperty damage) {
            npcObject.Health.AddPoints(-damage.value);
            AnimatorController.OnTakeDamage();
            if (npcObject.Health.CurrentPoints == 0) {
                AnimatorController.OnDie();
                transform.GetChild(0).GetComponent<MeshCollider>().enabled = false;
                AnimatorController = null;
            }
        }

        public void AttackTarget(ICombatTarget target) {
            AnimatorController.OnAttackMelee();
            StartCoroutine(DoDamage(target, 0.1f));
        }

        public IEnumerator DoDamage(ICombatTarget player, float delay) {
            yield return new WaitForSecondsRealtime(delay);
            player.TakeDamage(npcObject.Damage);
        }
    }
}
