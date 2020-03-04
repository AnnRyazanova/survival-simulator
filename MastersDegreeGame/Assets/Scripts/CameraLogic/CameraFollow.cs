using System;
using UnityEngine;

namespace CameraLogic
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private GameObject targetBody;
        private Vector3 _cameraOffset;

        private void Start() => _cameraOffset = transform.position - targetBody.transform.position;

        void Update() {
            if (targetBody != null) transform.position = targetBody.transform.position + _cameraOffset;
        }
    }
}