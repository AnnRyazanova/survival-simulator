using System;
using System.Linq;
using Objects;
using UnityEngine;

namespace Characters.Systems.Searching
{
    [Serializable]
    public class CircleRadarSystem
    {
        public float searchRadius;
        private Collider[] _colliders;

        public CircleRadarSystem(float radius = 3f, int preallocatedCollidersSize = 10) {
            _colliders = new Collider[preallocatedCollidersSize];
            searchRadius = radius;
        }

        public PickableItem FindClosestPickable(Transform self, LayerMask mask) {
            _colliders = Physics.OverlapSphere(self.position, searchRadius, mask);
            Collider closest = null;
            if (_colliders.Length > 0) {
                closest = _colliders.First();
                var position = self.position;
                foreach (var collider in _colliders) {
                    var distToPlayer = Vector3.SqrMagnitude(collider.transform.position - position);
                    var closestToPlayer = Vector3.SqrMagnitude(closest.transform.position - position);

                    if (distToPlayer < closestToPlayer) closest = collider;
                }
            }

            return closest != null ? closest.GetComponent<PickableItem>() : null;
        }
    }
}