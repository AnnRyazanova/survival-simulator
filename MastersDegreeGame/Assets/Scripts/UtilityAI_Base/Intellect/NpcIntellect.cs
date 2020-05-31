using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.NPC;
using Characters.Player;
using Prefabs.Monsters.Spider.AI;
using UnityEngine;
using UnityEngine.AI;
using UtilityAI_Base.Actions;
using UtilityAI_Base.Actions.Base;
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

        public ActionSelectorType actionSelectorType;
        public ActionSelectorType fallbackSelectorType;

        [SerializeField] public List<AbstractUtilityAction> actions = new List<AbstractUtilityAction>();
        [SerializeField] public List<AbstractUtilityAction> fallBackActions = new List<AbstractUtilityAction>();

        [HideInInspector] public ActionSelector fallbackSelector;
        [HideInInspector] public ActionSelector selector;

        #endregion

        #region Private members

        private AiContext _context;
        private NavMeshAgent _navMeshAgent;
        private UtilityPick _currentAction = null;

        private float _lastUpdated = 0f;
        private float _updateTimesPerSecond = 1;
        private bool _consecutiveActionsAreSame = false;
        private bool _inertiaIsApplied = false;
        #endregion

        #region public fields

        public float UpdateTimesPerSecond
        {
            get => _updateTimesPerSecond;
            set => _updateTimesPerSecond = Mathf.Clamp(value, 0f, 100f);
        }

        #endregion

        private void Awake() {
            _lastUpdated = Time.time + 1f / _updateTimesPerSecond;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _context = GetComponent<AiContext>();
            if (selector == null) selector = ActionSelectorFactory.GetSelector(actionSelectorType);
        }

        private void Update() {
            if (Time.time >= _lastUpdated) {
                _lastUpdated = Time.time + 1f / _updateTimesPerSecond;

                // Update context
                Sense();
                // Debug.Log("===============================================");
                // Think about next action to be taken
                Think();
                // Debug.Log("===============================================");

                // Perform selected action
                Act();
            }
        }

        #region Intellect methods

        private void Sense() {
            _context.UpdateContext();
        }

        private void Think() {
            // TODO: Null return checks for selectors and actions
            UtilityPick action = selector.Select(_context, actions);
            if (_currentAction == null) {
                if (action != null) _currentAction = action;
            }
            else {
                var currentActionScore = _currentAction.UtilityAction.EvaluateAbsoluteUtility(_context).Score;
                if (currentActionScore == 0) {
                    StartCoroutine(_currentAction.UtilityAction.SetInCooldown());
                    _currentAction = null;
                }

                if (action == null) return;
                if (_currentAction == null) {
                    _currentAction = action;
                }
                else {
                    _consecutiveActionsAreSame = action == _currentAction;
                    if (!_consecutiveActionsAreSame) {
                        if (action.Score > currentActionScore) {
                            StartCoroutine(_currentAction.UtilityAction.SetInCooldown());
                            _currentAction = action;
                        }
                    }
                }
            }
        }

        private void Act() {
            if (_currentAction != null) {
                Debug.Log(_currentAction.UtilityAction.description + " " + _currentAction.Score);
                _currentAction.UtilityAction.Execute(_context, _currentAction);
                if (!_consecutiveActionsAreSame) {
                    if (!_inertiaIsApplied) {
                        // StartCoroutine(_currentAction.UtilityAction.AddInertia());
                        _inertiaIsApplied = true;   
                    }
                }
                else {
                    _inertiaIsApplied = false;
                }
            }
        }

        #endregion

        public bool IsActive() {
            return true;
        }
    }
}