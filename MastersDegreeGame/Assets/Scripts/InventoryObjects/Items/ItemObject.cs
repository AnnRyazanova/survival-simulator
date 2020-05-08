using System;
using UnityEngine;
using Object = Characters.Object;

namespace InventoryObjects.Items
{
    public enum ItemObjectType
    {
        Consumable,
        Weapon,
        Material,
        Tool,
        Heat
    }

    public abstract class ItemObject : ScriptableObject
    {
        public ItemObjectType ItemType { get; protected set; }
        public Sprite displayIcon;
        public bool isStackable = true;
        public int maxItemsInStack = 10;
        public string title = "Предмет";
        public int id;
        
        public float maxDurability = 100f;
        public float durabilityDecreaseRate = 10f;

        
        [TextArea(20, 50)]
        public string description;

        public void Awake() {
            maxDurability = 100f;
            durabilityDecreaseRate = 1f;
        }
    }
}