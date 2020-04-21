using System;
using System.Linq;
using InventoryObjects.Items;
using Objects;
using UnityEngine;

namespace Characters.Systems
{
    [Serializable]
    public class ConeRadarSystem
    {
        private Collider[] _colliders;
        private float _radius;
        private float _angleCos;

        public ConeRadarSystem(float radius = 1.3f, float angle = 30f, int preAllocatedSize = 10) {
            _colliders = new Collider[preAllocatedSize];
            _radius = radius;
            _angleCos = Mathf.Cos(angle);
        }

        public PickableItem CheckForVisibleObjects(Transform transform) {
            var foundCount = Physics.OverlapSphereNonAlloc(transform.position, _radius, _colliders);
            if (foundCount == 0) return null;

            var mostForwardAngle = 0f;
            PickableItem result = null;

            foreach (var current in _colliders) {
                if (current.GetComponent<PickableItem>() == null) continue;

                var curDot = Vector3.Dot((current.transform.position - transform.position).normalized,
                    transform.forward);
                // If item is placed IN FRONT of a player
                if (curDot >= _angleCos) {
                    if (curDot > mostForwardAngle) {
                        mostForwardAngle = curDot;
                        result = current.GetComponent<PickableItem>();
                    }
                }
            }

            return result;
        }
    }
}