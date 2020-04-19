using System;
using System.Collections.Generic;
using InventoryObjects.Items;
using UnityEngine;

namespace InventoryObjects.Inventory
{
    [CreateAssetMenu(fileName = "Create New Inventory", menuName = "Inventory/Inventory")]
    public class Inventory : ScriptableObject
    {
        public int maxLength = 9;

        // public List<InventoryCell> container = new List<InventoryCell>();
        public InventoryCell[] container = new InventoryCell[9];

        public void AddItem(ItemObject item) {
            var firstFreeCellIdx = -1;
            for (var i = 0; i < maxLength; ++i) {
                if (container[i] != null && container[i].item != null) {
                    if (container[i].item.id == item.id && item.isStackable) {
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

        public void TidyLayout() {
            for (var i = 0; i < maxLength; i++) {
                if (container[i] == null) continue;
                for (var j = i - 1 < 0 ? 0 : i - 1; j >= 0; --j) {
                    if (container[j].item == null) {
                        if (j != 0 && container[j - 1].item == null) continue;
                        var tmp = container[i];
                        container[i] = container[j];
                        container[j] = tmp;
                    }
                }
            }
        }

        public void UpdateItems() {
            foreach (var cell in container) {
                cell.item.OnUpdate();
            }
        }
        
        public void RemoveItem(int itemId, int quantity = 1) {
            var containedItemIndex = Array.FindIndex(container, cell => cell.item != null && cell.item.id == itemId);
            if (containedItemIndex == -1) return;
            var amount = container[containedItemIndex].amount;
            if (amount > 1 && quantity < amount) {
                container[containedItemIndex].ReduceAmount(quantity);
            }
            else {
                container[containedItemIndex] = new InventoryCell(null, 0);
            }
        }

        public void Clear() {
            foreach (var inventoryCell in container) {
                inventoryCell.SetToNull();
            }
        }

        public bool HasItem(int itemId) =>
            Array.Find(container, cell => cell != null && cell.item.id == itemId) != null;
    }
}