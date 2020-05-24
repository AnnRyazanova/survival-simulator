using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UtilityAI_Base.Actions;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.Intellect.Interfaces;
using UtilityAI_Base.Selectors;
using UtilityAI_Base.Selectors.ActionSelectors;

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
        private UtilityAction _currentAction = null;

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
                
                UtilityAction action = selector.Select(_context, actions);
                if (action != null) {
                    if (_currentAction == null || _currentAction.Utility < action.Utility) {
                        StartCoroutine(_currentAction.SetInCooldown());
                        _currentAction = action;
                    }
                }

                if (_currentAction != null) {
                    _currentAction.Execute(_context);
                    StartCoroutine(_currentAction.AddInertia());
                }
            }
        }

        public bool IsActive() {
            return true;
        }
    }
}