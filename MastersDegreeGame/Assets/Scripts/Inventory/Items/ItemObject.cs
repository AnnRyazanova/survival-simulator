using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Items
{
    public enum ItemObjectType
    {
        Consumable,
        Weapon,
        Material
    }

    public abstract class ItemObject : ScriptableObject
    {
        public ItemObjectType ItemType { get; protected set; }
        
        public int id;
        public float durability;
        public float durabilityDecreaseRate;
        
        public abstract void OnUse(GameObject target);
    
        public abstract void OnThrowOut();
    
        public abstract void OnPickUp();
    
        public abstract void OnUpdate();
    }
}