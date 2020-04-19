using UnityEngine;

namespace InventoryObjects.Items
{
    public enum ItemObjectType
    {
        Consumable,
        Weapon,
        Material,
        Tool
    }

    public abstract class ItemObject : ScriptableObject
    {
        public ItemObjectType ItemType { get; protected set; }
        public Sprite displayIcon;
        public bool isStackable = true;
        
        [TextArea(20, 50)]
        public string description;
        
        public int id;
        public float durability;
        public float durabilityDecreaseRate;
        
        public abstract void OnUse(Object target);
    
        public abstract void OnThrowOut();
    
        public abstract void OnPickUp();
    
        public abstract void OnUpdate();
    }
}