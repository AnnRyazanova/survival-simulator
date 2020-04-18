using System.Collections.Generic;
using InventoryObjects.Items;
using UnityEngine;

namespace InventoryObjects.Inventory
{
    [CreateAssetMenu(fileName = "Create New InventoryController", menuName = "Inventory/InventoryController")]
    public class Inventory : ScriptableObject
    {
        public int maxLength = 9;

        public List<InventoryCell> container = new List<InventoryCell>();

        public void AddItem(ItemObject item) {
            var firstFreeCellIdx = -1;
            for (var i = 0; i < maxLength; ++i) {
                if (container[i] != null && container[i].item != null) {
                    if (container[i].item.id == item.id) {
                        container[i].AddAmount(1);
                        return;
                    }
                }
                else {
                    firstFreeCellIdx = firstFreeCellIdx == -1 ? i : firstFreeCellIdx;
                }
            }

            container[firstFreeCellIdx] = new InventoryCell(item, 1);
        }

        public void UpdateItems() {
            foreach (var cell in container) {
                cell.item.OnUpdate();
            }
        }

        public void RemoveItem(int itemId, int quantity = 1) {
            var containedItemIndex = container.FindIndex(cell => cell.item != null && cell.item.id == itemId);
            if (containedItemIndex == -1) return;
            var amount = container[containedItemIndex].amount;
            if (amount > 1 && quantity < amount) {
                container[containedItemIndex].ReduceAmount(quantity);
            }
            else {
                container[containedItemIndex] = new InventoryCell(null, 0);
            }
        }

        public InventoryCell GetItem(int id) => container.Find(cell => cell.item != null && cell.item.id == id);
        public void Clear() => container.Clear();
        public bool HasItem(int itemId) => container.Find(cell => cell != null && cell.item.id == itemId) != null;
    }
}