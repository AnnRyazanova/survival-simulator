using System;
using UnityEngine;

namespace CameraLogic
{
    public class CameraFollow : MonoBehaviour
    {
        public GameObject targetBody;
        private readonly Vector3 _cameraOffset = new Vector3(0, 7, -6);

        //private void Start() => _cameraOffset = transform.position - targetBody.transform.position;

        void Update() {
            if (targetBody != null) transform.position = targetBody.transform.position + _cameraOffset;
        }
    }
}