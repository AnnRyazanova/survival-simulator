using System;
using System.Collections;
using Characters.Animations;
using Characters.Controllers;
using Characters.Player;
using Characters.Systems.Combat;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Characters.NPC
{
    public class NpcMainScript : GameCharacter
    {
        [FormerlySerializedAs("mobObject")] public NpcObject npcObject;

        private NavMeshAgent _agent;

        private void Awake() {
            animatorController = new NpcAnimatorController(GetComponent<Animator>());
            _agent = GetComponent<NavMeshAgent>();
        }

        public void Start() {
            npcObject.animatorController = animatorController;
            attackRate = 1f;
        }

        public void tmpAiControllerFunc() {
            var hits = Physics.OverlapSphere(actionSphere.transform.position, itemSearchRadius);
            if (hits != null && animatorController != null) {
                foreach (var hit in hits) {
                    var hitGameObject = hit.gameObject;
                    if (hitGameObject != null && hitGameObject.GetComponent<PlayerMainScript>() != null) {
                        if (Time.time >= lastAttackTime) {
                            (npcObject as ICombatAggressor).AttackTarget(hitGameObject.GetComponent<PlayerMainScript>()
                                .playerObject);
                            lastAttackTime = Time.time + 1f / attackRate;
                        }
                    }
                }
            }
        }

        private void Update() {
            tmpAiControllerFunc();
            animatorController.OnMove(_agent.velocity.magnitude, 100);
        }
    }
}