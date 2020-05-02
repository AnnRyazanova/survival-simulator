﻿using Characters.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Controllers
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class CharacterMotor : MonoBehaviour
    {
        private RaycastHit _hitInfo;
        private NavMeshAgent _agent;
        private Rect _joystickPlacement = new Rect(200, 200, 256, 256);

        private void Start() {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update() {
            if (!Input.GetMouseButtonDown(0)) return;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Check raycast and that we didnt click on joystick
            if (Physics.Raycast(ray.origin, ray.direction, out _hitInfo) &&
                !_joystickPlacement.Contains(Input.mousePosition)) {
                _agent.destination = _hitInfo.point;
            }
        }

        private void OnAnimatorMove() {
            // Update position to agent position
            transform.position = _agent.nextPosition;
        }
    }
}