using System;
using InventoryObjects.Items;
using UnityEngine;

namespace InventoryObjects.Inventory
{
    [System.Serializable]
    public class InventoryCell: IComparable<InventoryCell>
    {
        public ItemObject item;
        public int amount;
        
        public InventoryCell(ItemObject item, int amount) {
            this.item = item;
            this.amount = amount;
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

        public int CompareTo(InventoryCell other) {
            if (ReferenceEquals(this, other)) return 0;
            return other?.amount.CompareTo(amount) ?? 1;
        }
    }
}