using System;
using System.Collections;
using Characters.Animations;
using Characters.Controllers;
using Characters.Player;
using Characters.Systems.Combat;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UtilityAI_Base.Agents.Interfaces;
using UtilityAI_Base.Contexts.Interfaces;
using UtilityAI_Base.Intellect;

namespace Characters.NPC
{
    public class NpcMainScript : GameCharacter, IAgent
    {
        [FormerlySerializedAs("mobObject")] public NpcObject npcObject;

        public NavMeshAgent _agent;
        private void Awake() {
            animatorController = new NpcAnimatorController(GetComponent<Animator>());
            _agent = GetComponent<NavMeshAgent>();
        }

        public void Start() {
            npcObject.animatorController = animatorController;
            attackRate = 1f;
        }

        public void OnStart(IAiContext owner) {
            var current = ((owner as SpiderContext)?.owner as NpcMainScript);
            if (current != null && current._agent != null) {
                current._agent.destination = PlayerMainScript.MyPlayer.transform.position;
            }
        }
        
        public void OnEnd(IAiContext owner) {
            var current = ((owner as SpiderContext)?.owner as NpcMainScript);
            if (current != null && current._agent != null) {
                current._agent.destination = new Vector3(10f, 0, 10f);
            }
        }
        
        public void TmpAiControllerFunc() {
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
            TmpAiControllerFunc();
            animatorController.OnMove(_agent.velocity.magnitude, 100);
        }

        public bool IsActive() {
            throw new NotImplementedException();
        }

        public Vector3 GetCurrentWorldPosition() {
            throw new NotImplementedException();
        }

        public Vector3 GetCurrentVelocity() {
            throw new NotImplementedException();
        }
    }
}