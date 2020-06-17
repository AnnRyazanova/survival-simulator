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
        
        [SerializeField] private float playerSearchRadius = 10f;
        [SerializeField] private float coverSearchRadius = 25f; 

        public DayNightCycleController dayNight;

        public LayerMask coverMask;

        private Collider[] _coverBuffer;

        public void ClearAll() {
            distancesToEnemies.Clear();
            distancesToCover.Clear();
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
            if (_foundCharactersCount > 0) {
                for (var i = 0; i < Colliders.Length; i++) {
                    if (Colliders[i] == null) continue;
                    var player = Colliders[i].gameObject.GetComponent<PlayerMainScript>();
                    if (player != null) {
                        target = player.gameObject;
                        break;
                    }
                }
            }
            
            if (_foundCoversCount == 0) return;
            ClearAll();
            for (var i = 0; i < _coverBuffer.Length; i++) {
                if (_coverBuffer[i] == null) continue;
                var selected = _coverBuffer[i].GetComponent<NpcCoverPoint>();
                if (selected != null) {
                    var position = selected.transform.position;
                    covers.Add(position);
                    distancesToCover.Add(Vector3.Distance(transform.position, position));
                }
            }
        }

        private void FixedUpdate() {
            var position = transform.position;
            _foundCharactersCount = Physics.OverlapSphereNonAlloc(position, playerSearchRadius, Colliders);
            _foundCoversCount = Physics.OverlapSphereNonAlloc(position, coverSearchRadius, _coverBuffer, coverMask);
        }


        [NpcContextVar]
        public float DistanceToEnemy =>
            target != null ? Vector3.Distance(transform.position, target.transform.position) : 100f;

        [NpcContextVar]
        public float TimeOfDay => DayNightCycleController.Get != null
            ? DayNightCycleController.Get.CurrentTimeOfDayFloat
            : 0f;


        [NpcContextVar] public float DistanceFromStartingPoint => Vector3.Distance(transform.position, StartingPoint);

        [HideInInspector] public List<Vector3> covers = new List<Vector3>();
        [HideInInspector] public List<float> distancesToEnemies = new List<float>();
        [HideInInspector] public List<float> distancesToCover = new List<float>();

        public override object GetParameter(AiContextVariable param) {
            switch (param) {
                case AiContextVariable.DistanceToTarget:
                    return DistanceToEnemy;
                case AiContextVariable.None:
                    return null;
                case AiContextVariable.Target:
                    return target;
                case AiContextVariable.Owner:
                    return owner;
                case AiContextVariable.DistancesToEnemies:
                    return distancesToEnemies;
                case AiContextVariable.DistanceFromStartingPoint:
                    return DistanceFromStartingPoint;
                case AiContextVariable.Covers:
                    return covers;
                case AiContextVariable.DistancesToCover:
                    return distancesToCover;
                case AiContextVariable.TimeOfDay:
                    return TimeOfDay;
                default:
                    throw new ArgumentOutOfRangeException(nameof(param), param, null);
            }
        }
    }
}