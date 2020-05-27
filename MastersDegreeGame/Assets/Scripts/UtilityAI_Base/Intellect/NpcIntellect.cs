using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UtilityAI_Base.Actions;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.Intellect.Interfaces;
using UtilityAI_Base.Selectors;
using UtilityAI_Base.Selectors.ActionSelectors;
using UtilityAI_Base.Selectors.Factories;

namespace UtilityAI_Base.Intellect
{
    [RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(AiContext))]
    public class NpcIntellect : MonoBehaviour, IIntellect
    {
        #region Public members

        [HideInInspector] public ActionSelectorType actionSelectorType = ActionSelectorType.HighestScoreWins;
        [HideInInspector] public ActionSelectorType fallbackSelectorType = ActionSelectorType.Random;

        [SerializeField] public List<UtilityAction> actions = new List<UtilityAction>();
        [SerializeField] public List<UtilityAction> fallBackActions = new List<UtilityAction>();
        
        [HideInInspector] public ActionSelector fallbackSelector = new RandomActionSelector();
        [HideInInspector] public ActionSelector selector = new HighestScoreWins();

        #endregion

        #region Private members

        private AiContext _context;
        private NavMeshAgent _navMeshAgent;
        private UtilityAction _currentAction = null;

        private float _lastUpdated = 0f;
        private float _updateTimesPerSecond = 1;

        #endregion
        
        #region public fields
        
        public float UpdateTimesPerSecond
        {
            get => _updateTimesPerSecond;
            set => _updateTimesPerSecond = Mathf.Clamp(value, 0f, 100f);
        } 
        
        #endregion
        
        private void Awake() {
            Debug.Log("Intellect " + actions.Count);
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _context = GetComponent<AiContext>();
        }
        
        private void Update() {
            if (Time.time >= _lastUpdated) {
                _lastUpdated = Time.time + 1f / _updateTimesPerSecond;

                // Update context
                Sense();
                // Think about next action to be taken
                Think();
                // Perform selected action
                Act();
            }
        }
        
        #region Intellect methods

        private void Sense() {
        }

        private void Think() {
            UtilityAction action = selector.Select(_context, actions);
            if (action != null) {
                if (_currentAction != null && _currentAction.Utility < action.Utility) {
                    StartCoroutine(_currentAction.SetInCooldown());
                    _currentAction = action;
                }
                else if (_currentAction == null) {
                    _currentAction = action;
                }
            }
            else {
                // if (_currentAction != null) StartCoroutine(_currentAction.SetInCooldown());
                // if (fallBackActions.Count > 0) _currentAction = fallbackSelector.Select(_context, fallBackActions);
            }
        }

        private void Act() {
            if (_currentAction != null) {
                _currentAction.Execute(_context);
                StartCoroutine(_currentAction.AddInertia());
            }
        }

        #endregion
        
        public bool IsActive() {
            return true;
        }
    }
}