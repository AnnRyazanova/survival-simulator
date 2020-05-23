using System;
using Characters.Player;
using UnityEngine;
using Object = Characters.Object;

namespace InventoryObjects.Items
{
    [CreateAssetMenu(fileName = "Create New Consumable", menuName = "Inventory/Items/Consumable")]
    public class ConsumableItem : ItemObject
    {
        public int healthRegen = 2;
        public int hungerRegen = 10;
        
        protected override void Awake() {
            base.Awake();
            ItemType = ItemObjectType.Consumable;
        }
    }
}