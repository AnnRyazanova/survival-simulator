using System;
using System.Collections.Generic;
using Characters.NPC;
using Characters.Player;
using UnityEngine;
using UtilityAI_Base.Agents.Interfaces;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.CustomAttributes;
using UtilityAI_Base.ResponseCurves;

namespace UtilityAI_Base.Intellect
{
    [NpcContext("Spider")]
    public class SpiderContext : AiContext
    {
        private Collider[] _colliders;
        public LayerMask visibleLayers;

        protected override void Awake() {
            base.Awake();
            PropertyValues = new Dictionary<string, float>();
            _colliders = new Collider[50];
            _td = new CurveParameter(0f, 0f, 10f);
            Fill();
        }

        private void Update() {
            foreach (var c in _colliders) {
                if (c != null) {
                    if (c.gameObject.GetComponent<PlayerMainScript>() != null) {
                        _targetPosition = c.transform.position;
                    }
                }
            }
            Fill();
        }

        private void FixedUpdate() {
            Physics.OverlapSphereNonAlloc(transform.position, 10f, _colliders, visibleLayers);
        }

        public Vector3 _targetPosition;
        private CurveParameter _td;
        
        [NpcContextVar]
        public float EnemiesNearby { get; set; }
        
        [NpcContextVar]
        public float Energy { get; set; }
        
        [NpcContextVar]
        public float DistanceToTarget
        {
            get
            {
                _td.Value = Vector3.Distance(transform.position, _targetPosition);
                return _td.Value / _td.MaxValue;
            }
        }
    }
}