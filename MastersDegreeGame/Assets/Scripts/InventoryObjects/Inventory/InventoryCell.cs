using InventoryObjects.Items;
using UnityEngine;

namespace InventoryObjects.Inventory
{
    [System.Serializable]
    public class InventoryCell
    {
        public ItemObject item;
        public int Amount { get; private set; }

        public InventoryCell(ItemObject item, int amount) {
            this.item = item;
            this.Amount = amount;
        }

        public void AddAmount(int value) => Amount += value;

        public void ReduceAmount(int value) {
            if (Amount >= value) {
                Amount -= value;
            }
            else {
                Amount = 0;
            }
        }
    }
}