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
        private int foundCollisionsCount = 0;

        public void ClearAll() {
            Enemies.Clear();
            DistancesToEnemies.Clear();
        }

        private void Awake() {
            owner = GetComponent<NpcMainScript>();
            target = null;
            Colliders = new Collider[bufferSize];
        }

        public override void UpdateContext() {
            if (foundCollisionsCount == 0) return;

            if (Enemies.Count > 0) ClearAll();

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
        }

        private void FixedUpdate() {
            foundCollisionsCount = Physics.OverlapSphereNonAlloc(transform.position, 10f, Colliders);
        }

        private float _distanceToEnemy = 100f;

        [NpcContextVar]
        public float DistanceToEnemy
        {
            get => target != null ? Vector3.Distance(transform.position, target.transform.position) : 100f;
            set => _distanceToEnemy = value;
        }

        [NpcContextVar] public float QuantityEnemiesNearby { get; set; }

        [HideInInspector] public List<ICombatTarget> Enemies = new List<ICombatTarget>();
        [HideInInspector] public List<float> DistancesToEnemies = new List<float>();

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
                default:
                    throw new ArgumentOutOfRangeException(nameof(param), param, null);
            }
        }
    }
}