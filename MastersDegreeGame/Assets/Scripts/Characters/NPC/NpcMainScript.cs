using System;
using System.Collections;
using Characters.Animations;
using Characters.Controllers;
using Characters.Player;
using Characters.Systems.Combat;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UtilityAI_Base;
using UtilityAI_Base.Agents.Interfaces;
using UtilityAI_Base.Contexts;
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

        public void GoToTarget(AiContext context, UtilityPick pick) {
            var hitGameObject = context.target;
            var owner = context.owner as NpcMainScript;
            owner._agent.SetDestination(hitGameObject.transform.position);
        }

        public void ReturnHome(AiContext context, UtilityPick pick) {
            var owner = context.owner as NpcMainScript;
            owner._agent.SetDestination(context.StartingPoint);
        }
        
        public void AttackPlayer(AiContext context, UtilityPick pick) {
            var hitGameObject = context.target;
            var owner = context.owner as NpcMainScript;
            owner._agent.updateRotation = false;
            
            Vector3 direction = (hitGameObject.transform.position - owner.transform.position).normalized;
            Quaternion  qDir= Quaternion.LookRotation(direction);
            
            owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, qDir, Time.deltaTime * 100f);
            
            owner._agent.updateRotation = true;

            // if (hitGameObject != null && hitGameObject.GetComponent<PlayerMainScript>() != null) {
            //     (owner.npcObject as ICombatAggressor)?.AttackTarget(hitGameObject.GetComponent<PlayerMainScript>()
            //         .playerObject);
            // }
        }
        
        private void Update() {
            animatorController.OnMove(_agent.velocity.magnitude, 100);
        }

        public bool IsActive() {
            return true;
        }

        public Vector3 GetCurrentWorldPosition() {
            return transform.position;
        }

        public Vector3 GetCurrentVelocity() {
            return _agent.velocity;
        }
    }
}