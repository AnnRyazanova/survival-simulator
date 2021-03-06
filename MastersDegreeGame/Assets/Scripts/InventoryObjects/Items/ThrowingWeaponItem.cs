﻿using UnityEngine;
using Object = Characters.Object;

namespace InventoryObjects.Items
{
    // Weapon items can be used as tools
    [CreateAssetMenu(fileName = "Create New Weapon", menuName = "Inventory/Items/Throwable")]
    public class ThrowingWeaponItem : ItemObject
    {
        public int attackPower;

        public GameObject weaponPrefab;
        
        protected override void Awake() 
        {
            base.Awake();
            ItemType = ItemObjectType.Weapon;
            isStackable = true;
        }
    }
}