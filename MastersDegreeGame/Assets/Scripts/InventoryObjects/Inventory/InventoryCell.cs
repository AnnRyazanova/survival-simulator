using InventoryObjects.Items;
using UnityEngine;

namespace InventoryObjects.Inventory
{
    [System.Serializable]
    public class InventoryCell
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
    }
}