using System;
using System.Collections.Generic;
using Characters.NPC;
using Characters.Player;
using Characters.Systems.Combat;
using UnityEngine;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.CustomAttributes;

namespace Prefabs.Monsters.Spider.AI
{
    [Serializable]
    public class SpiderContext : AiContext
    {
        private int _foundCharactersCount = 0;
        private int _foundCoversCount = 0;

        public DayNightCycleController dayNight;

        public LayerMask coverMask;

        private Collider[] _coverBuffer;

        public void ClearAll() {
            Enemies.Clear();
            DistancesToEnemies.Clear();
            DistancesToCover.Clear();
            CoverValues.Clear();
        }

        private void Awake() {
            owner = GetComponent<NpcMainScript>();
            target = null;
            Colliders = new Collider[bufferSize];
            _coverBuffer = new Collider[bufferSize];
            StartingPoint = ((NpcMainScript) owner).transform.position;
            dayNight = DayNightCycleController.Get;
        }

        public override void UpdateContext() {
            if (_foundCharactersCount == 0) return;
            ClearAll();
            _timeSinceLastWander = DistanceFromStartingPoint == 0 ? _timeSinceLastWander + 0.01f : 0f;
            foreach (var col in Colliders) {
                if (col == null) continue;
                var obj = col.gameObject;
                var parent = obj.transform.parent;
                if (parent != null) {
                    var npc = parent.GetComponent<NpcMainScript>();
                    if (npc != null && npc != (owner as NpcMainScript)) {
                        Enemies.Add(npc.npcObject);
                        DistancesToEnemies.Add(Vector3.Distance(transform.position, npc.transform.position));
                    }
                }

                if (obj.GetComponent<PlayerMainScript>() != null) {
                    target = obj.GetComponent<PlayerMainScript>().gameObject;
                }
            }

            if (_foundCoversCount == 0) return;
            foreach (var cover in _coverBuffer) {
                if (cover == null) continue;
                var selected = cover.GetComponent<NpcCoverPoint>();
                if (selected != null) {
                    var position = selected.transform.position;
                    Covers.Add(position);
                    DistancesToCover.Add(Vector3.Distance(transform.position, position));
                    CoverValues.Add(selected.value);
                }
            }
        }

        private void FixedUpdate() {
            var position = transform.position;
            _foundCharactersCount = Physics.OverlapSphereNonAlloc(position, 10f, Colliders);
            _foundCoversCount = Physics.OverlapSphereNonAlloc(position, 20f, _coverBuffer, coverMask);
        }

        private float _distanceToEnemy = 100f;
        private float _timeSinceLastWander = 0f;

        [NpcContextVar]
        public float DistanceToEnemy
        {
            get => target != null ? Vector3.Distance(transform.position, target.transform.position) : 100f;
            set => _distanceToEnemy = value;
        }

        [NpcContextVar]
        public float TimeOfDay =>
            DayNightCycleController.Get != null ? DayNightCycleController.Get._currentTimeOfDay : 0.5f;

        [NpcContextVar] public float DistanceFromStartingPoint => Vector3.Distance(transform.position, StartingPoint);
        [NpcContextVar] public float QuantityEnemiesNearby { get; set; }

        public float TimeSinceLastWander => _timeSinceLastWander;

        [HideInInspector] public List<ICombatTarget> Enemies = new List<ICombatTarget>();
        [HideInInspector] public List<Vector3> Covers = new List<Vector3>();

        [HideInInspector] public List<float> DistancesToEnemies = new List<float>();

        [HideInInspector] public List<float> DistancesToCover = new List<float>();
        [HideInInspector] public List<float> CoverValues = new List<float>();

        public override object GetParameter(AiContextVariable param) {
            switch (param) {
                case AiContextVariable.DistanceToTarget:
                    return DistanceToEnemy;
                case AiContextVariable.Enemies:
                    return Enemies;
                case AiContextVariable.None:
                    return null;
                case AiContextVariable.Target:
                    return target;
                case AiContextVariable.Owner:
                    return owner;
                case AiContextVariable.DistancesToEnemies:
                    return DistancesToEnemies;
                case AiContextVariable.DistanceFromStartingPoint:
                    return DistanceFromStartingPoint;
                case AiContextVariable.Covers:
                    return Covers;
                case AiContextVariable.DistancesToCover:
                    return DistancesToCover;
                case AiContextVariable.CoverValues:
                    return CoverValues;
                case AiContextVariable.TimeOfDay:
                    return TimeOfDay;
                default:
                    throw new ArgumentOutOfRangeException(nameof(param), param, null);
            }
        }
    }
}