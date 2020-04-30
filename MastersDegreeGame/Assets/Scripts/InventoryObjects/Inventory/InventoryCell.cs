using System;
using Characters.Player;
using InventoryObjects.Items;
using UnityEngine;
using Object = System.Object;

namespace InventoryObjects.Inventory
{
    [System.Serializable]
    public class InventoryCell: IComparable<InventoryCell>
    {
        public ItemObject item;
        public int amount;
        private float _durability;

        public float NormalizedCurrentDurability => _durability / item.maxDurability;
        
        public InventoryCell(ItemObject item, int amount) {
            this.item = item;
            this.amount = amount;
            
            _durability = item == null ? 0 : item.maxDurability;
        }

        public void SetToNull() {
            item = null;
            amount = 0;
        }
        
        public void AddAmount(int value) => amount += value;

        public void ReduceAmount(int value) {
            if (amount >= value) {
                amount -= value;
            }
            else {
                amount = 0;
            }
        }

        public void Update() {
            // if (item.ItemType == ItemObjectType.Consumable) {
                _durability -= item.durabilityDecreaseRate;
                _durability = _durability <= 0 ? 0 : _durability;
            // }
        }

        public void Use(Object target) {
            if (item is ConsumableItem consumable) {
                if (target is PlayerObject playerObject) {
                    var healthRegen = _durability > 10 ? consumable.healthRegen : -consumable.healthRegen;
                    var hungerRegen = _durability > 10 ? consumable.hungerRegen : -consumable.hungerRegen;
                    
                    playerObject.Health.AddPoints(healthRegen);
                    playerObject.Hunger.AddPoints(hungerRegen);
                }
            }
        }
        
        public int CompareTo(InventoryCell other) {
            if (ReferenceEquals(this, other)) return 0;
            return other?.amount.CompareTo(amount) ?? 1;
        }
    }
}