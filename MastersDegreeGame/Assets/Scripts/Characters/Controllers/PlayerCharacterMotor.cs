using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Controllers
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class PlayerCharacterMotor : MonoBehaviour
    {
        private FixedJoystick directionalJoystick;
        private NavMeshAgent _agent;
        private bool _isInited;
        private Vector2 _inputDirections;

        public Vector3 destinationPoint = Vector3.zero;

        private void Awake() {
            StartCoroutine(InitJoystick());
            _agent = GetComponent<NavMeshAgent>();

        }

        private void Update() {
            if (_isInited) {
                _inputDirections = new Vector2(directionalJoystick.Horizontal, directionalJoystick.Vertical);
                if (_inputDirections != Vector2.zero) {
                    var movement = (Vector3.forward * _inputDirections.y + Vector3.right * _inputDirections.x)
                        .normalized;
                    destinationPoint = movement + transform.position;
                }
                _agent.destination = destinationPoint;
            }
        }

        private IEnumerator InitJoystick() {
            directionalJoystick = MainWindowController.Instance.GetJoystick();

            while (directionalJoystick == null) {
                yield return new WaitForSeconds(.1f);
                directionalJoystick = MainWindowController.Instance.GetJoystick();
            }

            _isInited = true;
        }

        private void OnAnimatorMove() {
            // Update position to agent position
            transform.position = _agent.nextPosition;
        }
    }
}