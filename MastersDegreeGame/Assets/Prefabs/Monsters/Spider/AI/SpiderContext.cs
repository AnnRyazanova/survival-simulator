using System;
using Characters.Player;
using UnityEngine;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.CustomAttributes;

namespace Prefabs.Monsters.Spider.AI
{
    [Serializable]
    public class SpiderContext : AiContext
    {
        public Collider[] colliders;

        public void Update() {
            foreach (var col in colliders) {
                if (col != null) {
                    var obj = col.gameObject;
                    if (obj.GetComponent<PlayerMainScript>() != null) {
                        target = obj.GetComponent<PlayerMainScript>().gameObject;
                    }
                }
            }
        }

        private void FixedUpdate() {
            colliders = Physics.OverlapSphere(transform.position, 10f);
        }

        private float _distanceToEnemy = 100f;
        [NpcContextVar]
        public float DistanceToEnemy
        {
            get => target != null ? Vector3.Distance(transform.position, target.transform.position) : 100f;
            set => _distanceToEnemy = value;
        }

        [NpcContextVar] public float QuantityEnemiesNearby { get; set; }
        public override object GetParameter(AiContextVariable param) {
            switch (param) {
                case AiContextVariable.DistanceToTarget:
                    return DistanceToEnemy;
                case AiContextVariable.Enemies:
                    return QuantityEnemiesNearby;
                case AiContextVariable.None:
                    return null;
                case AiContextVariable.Target:
                    return target;
                case AiContextVariable.Owner:
                    return owner;
                default:
                    throw new ArgumentOutOfRangeException(nameof(param), param, null);
            }
        }
    }
}