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
        public int maxItemsInStack = 10;
        public string title = "Предмет";
        public int id;
        public float durability;
        public float durabilityDecreaseRate;
        
        [TextArea(20, 50)]
        public string description;
        
        public abstract void OnUse(Object target);
    
        public abstract void OnThrowOut();
    
        public abstract void OnPickUp();
    
        public abstract void OnUpdate();
    }
}