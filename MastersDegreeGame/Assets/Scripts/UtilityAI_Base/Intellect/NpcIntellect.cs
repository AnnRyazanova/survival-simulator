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
        [SerializeField] public List<AtomicUtilityAction> fallBackActions = new List<AtomicUtilityAction>();

        [HideInInspector] public ActionSelector fallbackSelector;
        [HideInInspector] public ActionSelector selector;

        #endregion

        #region Private members

        private AiContext _context;
        private NavMeshAgent _navMeshAgent;
        private UtilityPick _currentAction = null;
        
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
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _context = GetComponent<AiContext>();
            if (selector == null) selector = ActionSelectorFactory.GetSelector(actionSelectorType);
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
            _context.UpdateContext();
        }

        private void Think() {
            UtilityPick action = selector.Select(_context, actions);
            _currentAction = action;

            // if (action != null) {
            //     _currentAction = action;
            //
            //     // if (_currentAction != null && _currentAction.Utility < action.Utility) {
            //     //     StartCoroutine(_currentAction.SetInCooldown());
            //     //     _currentAction = action;
            //     // }
            //     // if (_currentAction == null) {
            //     //     _currentAction = action;
            //     // }
            // }
            // else {
            //     // if (_currentAction != null) StartCoroutine(_currentAction.SetInCooldown());
            //     // if (fallBackActions.Count > 0) _currentAction = fallbackSelector.Select(_context, fallBackActions);
            // }
        }

        private void Act() {
            if (_currentAction != null) {
                Debug.Log(_currentAction.UtilityAction.description + " " + _currentAction.ActionType);
                // _currentAction.UtilityAction.Execute(_context, _currentAction);
                StartCoroutine(_currentAction.UtilityAction.AddInertia());
            }
        }

        #endregion
        
        public bool IsActive() {
            return true;
        }
    }
}