﻿using System;
using System.Collections;
using Characters.Animations;
using Characters.Controllers;
using Characters.NPC;
using Characters.Player;
using InventoryObjects.Inventory;
using InventoryObjects.Items;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace Characters.Systems.Combat
{
    [RequireComponent(typeof(GameCharacter))]
    public class RangedAttacker : MonoBehaviour
    {
        private RaycastHit _hit;
        private RaycastHit _directRay;
        private CharacterAnimatorController _animatorController;

        private Object _ownerObject;
        private Vector3 _myPosition;
        private Vector3 _npcPosition;
        private float _lastAttackTime;

        public float maxDistanceToTarget = 6f;
        public float minDistanceToTarget = 1f;
        public float attackSpeedPerSecond = 1f;
        public float projectileSpeed = 30f;

        public LayerMask rayCastLayer;
        public InventoryCell _rangedWeapon;
        public GameObject throwingPoint;
        public Quaternion instanceAngles = Quaternion.Euler(90, 0, 0);
        
        private void Start() {
            instanceAngles = Quaternion.Euler(90, 180, -10);
            var playerMainScript = GetComponent<PlayerMainScript>();
            if (playerMainScript != null) {
                _animatorController = playerMainScript.animatorController;
                _rangedWeapon = playerMainScript.equipment.weapon;
                _ownerObject = playerMainScript.playerObject;
            }
            else {
                var npc = GetComponent<NpcMainScript>();
                _animatorController = npc.animatorController;
                _ownerObject = npc.npcObject;
                // Set ranged to null, because enemies do not have a projectile limit
                _rangedWeapon = null;
            }

            _lastAttackTime = Time.time + 1f / attackSpeedPerSecond;
        }


        private void Update() {
            if (Input.GetMouseButtonDown(0) && Time.time >= _lastAttackTime) {
                FindAndAttackTarget();
                _lastAttackTime = Time.time + 1f / attackSpeedPerSecond;
            }
        }

        private IEnumerator AttackRanged(ICombatTarget target) {
            var position = _hit.transform.position;
            NavMeshController.LookAt(transform, position - _myPosition);
            _animatorController.OnAttackRanged();
            yield return new WaitForSeconds(.4f);

            var instance = Instantiate((_rangedWeapon.item as ThrowingWeaponItem)?.weaponPrefab,
                throwingPoint.transform.position, instanceAngles);
            instance.transform.parent = throwingPoint.transform;
            var rigidbody = instance.GetComponent<Rigidbody>();

            rigidbody.isKinematic = false;

            rigidbody.AddForce((position - _myPosition).normalized * projectileSpeed,
                ForceMode.Impulse);

            (_ownerObject as ICombatAggressor)?.AttackTarget(target);

            _rangedWeapon.ReduceAmount(1);
            if (_rangedWeapon.amount == 0) {
                _rangedWeapon.item = null;
                PlayerMainScript.MyPlayer.UnequipWeapon();
            }
        }

        // TODO: Scale to NPC-Player attacking
        private void FindAndAttackTarget() {
            if (_rangedWeapon != null && _rangedWeapon.item != null) {
                _myPosition = transform.position;
                var clickedPosition = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(clickedPosition.origin, clickedPosition.direction, out _hit)) {
                    if (Physics.Linecast(_myPosition, _hit.point, out _directRay, rayCastLayer)) {
                        if (_directRay.distance <= maxDistanceToTarget) {
                            Debug.Log(_directRay.distance);
                            var parent = _directRay.collider.gameObject.transform.parent;
                            var npc = parent != null ? parent.GetComponent<NpcMainScript>() : null;
                            if (npc != null) {
                                Debug.Log(npc.name);
                                StartCoroutine(AttackRanged(npc.npcObject));
                            }
                        }
                    }
                }
            }
        }
    }
}