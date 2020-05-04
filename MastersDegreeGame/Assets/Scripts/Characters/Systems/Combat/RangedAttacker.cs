using Characters.Animations;
using Characters.Controllers;
using Characters.NPC;
using Characters.Player;
using InventoryObjects.Inventory;
using UnityEngine;

namespace Characters.Systems.Combat
{
    [RequireComponent(typeof(GameCharacter))]
    public class RangedAttacker : MonoBehaviour
    {
        private RaycastHit _hit;
        private RaycastHit _directRay;
        private CharacterAnimatorController _animatorController;
        private InventoryCell _rangedWeapon;
        private NpcObject _targetObject;
        private Vector3 _myPosition;
        private Vector3 _npcPosition;
        private float _lastAttackTime;
        
        public float maxDistanceToTarget = 100f;
        public float minDistanceToTarget = 1f;
        public float attackSpeedPerSecond = 1f;
        public float projectileSpeed = 10f;
        public LayerMask rayCastLayer;
        
        private void Start() {
            var playerMainScript = GetComponent<PlayerMainScript>();
            if (playerMainScript != null) {
                _animatorController = playerMainScript.animatorController;
                _rangedWeapon = playerMainScript.equipment.weapon;
            }
            else {
                _animatorController = GetComponent<NpcMainScript>().animatorController;
                // Set ranged to null, because enemies do not have a projectile limit
                _rangedWeapon = null;
            }
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0) && Time.time >= _lastAttackTime) {
                AttackRanged();
                _lastAttackTime = Time.time + 1f / attackSpeedPerSecond;
            }
        }


        private void AttackRanged() {
            _myPosition = transform.position;
            var clickedPosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(clickedPosition.origin, clickedPosition.direction, out _hit)) {
                if (Physics.Linecast(_myPosition, _hit.point, out _directRay, rayCastLayer)) {
                    var parent = _directRay.collider.gameObject.transform.parent;
                    var npc = parent != null ? parent.GetComponent<NpcMainScript>() : null;
                    if (npc != null) {
                        NavMeshController.LookAt(transform, _hit.point - _myPosition);
                        _animatorController.OnAttackRanged();
                    }
                }
            }
            else {
                _targetObject = null;
            }
        }
    }
}