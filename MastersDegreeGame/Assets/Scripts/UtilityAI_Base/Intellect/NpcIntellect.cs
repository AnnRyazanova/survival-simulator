using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UtilityAI_Base.Actions;
using UtilityAI_Base.Agents.Interfaces;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.Intellect.Interfaces;
using UtilityAI_Base.Selectors;

namespace UtilityAI_Base.Intellect
{
    [RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(AiContext))]
    public class NpcIntellect : MonoBehaviour, IIntellect
    {
        [SerializeField] public List<UtilityAction> actions = new List<UtilityAction>();
        [SerializeField] private ActionSelector selector = new HighestScoreWins();
        
        [SerializeField] private float updateTimesPerSecond = 1f;
        
        private AiContext _context;
        private NavMeshAgent _navMeshAgent;
        private float _lastUpdated = 0f;
        
        private void Awake() {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _context = GetComponent<AiContext>();
        }

        private void Update() {
            if (Time.time >= _lastUpdated) {
                _lastUpdated = Time.time + 1f / updateTimesPerSecond;
                
                // UPDATE HERE
                
                // CALL ACTION SELECTION HERE
                
                UtilityAction currentAction = selector.Select(_context, actions);
                if (currentAction != null && currentAction.CanBeInvoked()) {
                    currentAction.Execute(_context);
                }
            }
        }

        public bool IsActive() {
            return true;
        }
    }
}