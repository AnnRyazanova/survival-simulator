﻿using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Animations;
using Characters.Controllers;
using Characters.NPC;
using Characters.Systems;
using Characters.Systems.Combat;
using Characters.Systems.Searching;
using InventoryObjects.Inventory;
using InventoryObjects.Items;
using Objects;
using UnityEngine;
using UnityEngine.AI;
using UtilityAI_Base.Actions;
using UtilityAI_Base.Agents.Interfaces;
using UtilityAI_Base.Intellect;


namespace Characters.Player
{
    public class PlayerMainScript : GameCharacter, IAgent
    {
        public FixedJoystick directionalJoystick;
        public PlayerObject playerObject;
        public static PlayerMainScript MyPlayer { get; private set; }
        
        public GameObject leftHand;
        public GameObject rightHand;

        public Inventory inventory;
        public Equipment equipment;
        
        public float hitDelaySeconds = 0.1f;
        
        public ConeRadarSystem coneRadarSystem;
        public CircleRadarSystem circleRadar;

        private Collider[] _NPCsAround;
        private Collider[] _NPCsPrevious;
        
        public float npcActivationRadius = 30f;
        public int foundNpcCount = 0;
        
        public LayerMask pickableMask;
        public LayerMask enemiesMask;
        
        public bool isRangedEquipped;
        
        private Vector2 _inputDirections = Vector2.zero;
        private bool _isInited;
        
        public void InteractWithClosestItem() {
            // var state = animatorController.AnimatorController.GetCurrentAnimatorStateInfo(0);
            var nearest = coneRadarSystem.CheckForVisibleObjects(transform);
            if (nearest != null) {
                animatorController.OnPickup();
                var indexToAddTo = inventory.FindFreeCellToAdd(nearest.item);
                if (indexToAddTo != -1) {
                    if (nearest.isPickable) {
                        inventory.AddItem(nearest.item, indexToAddTo);
                        Destroy(nearest.gameObject);
                    }
                }
            }
        }

        private void OnApplicationQuit() {
            inventory.Clear();
            equipment.weapon = null;
            equipment.tool = null;
        }

        private void ActivateNPCs() {
            if (_NPCsPrevious != null && _NPCsPrevious.Length > 0 && _NPCsPrevious != _NPCsAround) {
                for (var i = 0; i < _NPCsPrevious.Length; i++) {
                    _NPCsPrevious[i].transform.parent.GetComponent<NpcIntellect>().isEnabled = false;
                }
            }

            if (_NPCsAround.Length > 0) {
                for (var i = 0; i < _NPCsAround.Length; i++) {
                    _NPCsAround[i].transform.parent.GetComponent<NpcIntellect>().isEnabled = true;
                }
            }
            _NPCsPrevious = _NPCsAround;
        }
        
        private void FixedUpdate() {
            _NPCsAround = Physics.OverlapSphere(transform.position, npcActivationRadius, enemiesMask);
        }

        private void Awake() {
            coneRadarSystem = new ConeRadarSystem();
            circleRadar = new CircleRadarSystem();
            animatorController = new PlayerAnimatorController(GetComponent<Animator>());
            NavMeshController = new NavMeshController(GetComponent<NavMeshAgent>());
            StartCoroutine(InitJoystick());
            GetComponent<RangedAttacker>().enabled = false;
            _NPCsAround = new Collider[20];
        }

        private void Start() {
            MyPlayer = this;
            playerObject.animatorController = animatorController;
        }

        #region Combat

        public void Attack() {
            if (equipment.weapon.item != null) {
                if (equipment.weapon.item is WeaponItem melee) {
                    AttackMelee();
                }
                else {
                    // Defaults to ThrowingWeaponItem
                }
            }
        }

        public void AttackMelee() {
            animatorController.OnAttackMelee();
            var hits = Physics.OverlapSphere(actionSphere.transform.position, itemSearchRadius);
            if (hits != null) {
                foreach (var hit in hits) {
                    var colliderParent = hit.gameObject.transform.parent;
                    if (colliderParent != null) {
                        if (colliderParent.GetComponent<NpcMainScript>() != null) {
                            playerObject.AttackTarget(colliderParent.GetComponent<NpcMainScript>().npcObject);
                        }
                    }
                }
            }
        }

        public void AttackRanged() {
            if (!Input.GetMouseButtonDown(0)) return;
            RaycastHit hitInfo;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo)) {
                var npc = hitInfo.collider.gameObject.transform.parent.GetComponent<NpcMainScript>();
                if (npc != null) {
                    NavMeshController.LookAt(transform, npc.transform.position);
                    animatorController.OnAttackRanged();
                }
            }
        }

        #endregion
        
        public void OnDrawGizmosSelected() 
        {
            Gizmos.DrawWireSphere(actionSphere.transform.position, itemSearchRadius);
            Gizmos.DrawWireSphere(transform.position, 10f);
        }

        private void Update() {
            if (_isInited == false) return;
            _inputDirections = new Vector2(directionalJoystick.Horizontal, directionalJoystick.Vertical);
            NavMeshController.Move(transform, _inputDirections,
                playerObject.Energy.CurrentPoints > 0 ? characterRunSpeed : characterWalkSpeed);
            
            animatorController.OnMove(_inputDirections.magnitude, playerObject.Energy.CurrentPoints);
            
            inventory.UpdateItems();
            ActivateNPCs();
        }

        private IEnumerator InitJoystick() {
            directionalJoystick = MainWindowController.Instance.GetJoystick();

            while (directionalJoystick == null) {
                yield return new WaitForSeconds(.1f);
                directionalJoystick = MainWindowController.Instance.GetJoystick();
            }

            _isInited = true;
        }

        #region EquipmentActions

        private static GameObject InstantiateEquipmentPrefab(GameObject hand, GameObject itemPrefab) {
            var handTransform = hand.transform;
            if (handTransform.childCount > 0) {
                Destroy(handTransform.GetChild(0).gameObject);
            }

            var instance = Instantiate(itemPrefab);
            instance.GetComponent<PickableItem>().isPickable = false;
            return instance;
        }

        private void EquipOnPrefab(GameObject hand, GameObject itemPrefabInstance) {
            itemPrefabInstance.transform.parent = hand.transform;
            itemPrefabInstance.transform.localPosition = Vector3.zero;
            if (hand == rightHand) {
                itemPrefabInstance.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
        }

        private static void UnequipOnPrefab(GameObject hand) {
            foreach (Transform child in hand.transform) {
                Destroy(child.gameObject);
            }
        }

        public void UnequipWeapon() {
            isRangedEquipped = false;
            GetComponent<RangedAttacker>().enabled = false;
            UnequipOnPrefab(rightHand);
        }

        public void UnequipTool() {
            UnequipOnPrefab(leftHand);
            leftHand.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        }

        public void EquipWeapon() {
            if (equipment.weapon != null) {
                // Instantiate prefab on scene
                GameObject instance = null;
                if (equipment.weapon.item is WeaponItem weapon) {
                    instance = InstantiateEquipmentPrefab(rightHand, weapon.weaponPrefab);
                    playerObject.Damage.value = weapon.attackPower;
                }
                else if (equipment.weapon.item is ThrowingWeaponItem throwingWeapon) {
                    instance = InstantiateEquipmentPrefab(rightHand, throwingWeapon.weaponPrefab);
                    playerObject.Damage.value = throwingWeapon.attackPower;
                    // TODO: Refactor and null checks
                    isRangedEquipped = true;
                    GetComponent<RangedAttacker>().enabled = true;
                }

                EquipOnPrefab(rightHand, instance);
            }
        }

        public void EquipTool() {
            if (equipment.tool != null) {
                // Instantiate prefab on scene
                var instance = InstantiateEquipmentPrefab(leftHand, (equipment.tool.item as ToolItem)?.toolPrefab);
                EquipOnPrefab(leftHand, instance);
                instance.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        #endregion

        public bool IsActive() {
            return true;
        }

        public Vector3 GetCurrentWorldPosition() {
            return transform.position;
        }

        public Vector3 GetCurrentVelocity() {
            // TODO: change to nvmesh
            return Vector3.forward;
        }
    }
}