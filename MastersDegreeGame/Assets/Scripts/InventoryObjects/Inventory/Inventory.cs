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

        public InventoryCell[] container = new InventoryCell[9];
        private float _lastUpdateTime = 0f;
        [SerializeField] private float updatesInSecond = 1f;

        public void AddItem(ItemObject item, int atIndex = -1, int quantity = 1) {
            var firstFreeCellIdx = atIndex == -1 ? FindFreeCellToAdd(item) : atIndex;
            if (container[firstFreeCellIdx].item != null
                && container[firstFreeCellIdx].item.ItemType == item.ItemType) {
                container[firstFreeCellIdx].AddAmount(quantity);
            }
            else {
                container[firstFreeCellIdx] = new InventoryCell(item, quantity);
            }
        }

        public void AddCell(InventoryCell cell, int atIndex = -1, int quantity = 1) {
            for (var i = 0; i < container.Length; ++i) {
                if (container[i] == null || container[i].item == null) {
                    container[i] = cell;
                    break;
                }
            }
        }

        #region Helpers

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

        public int FindFreeCellToAdd(ItemObject item) {
            var firstFreeCellIdx = -1;
            for (var i = 0; i < maxLength; ++i) {
                if (container[i] != null && container[i].item != null) {
                    var canStack = item.isStackable && container[i].amount < item.maxItemsInStack;
                    if (container[i].item.id == item.id && canStack) {
                        return i;
                    }
                }
                else {
                    firstFreeCellIdx = firstFreeCellIdx == -1 ? i : firstFreeCellIdx;
                }
            }

            return firstFreeCellIdx;
        }

        #endregion

        public void UpdateItems() {
            if (Time.time >= _lastUpdateTime) {
                foreach (var cell in container) {
                    if (cell == null || cell.item == null) continue;
                    cell.Update();
                    _lastUpdateTime = Time.time + 1f / updatesInSecond;
                }    
            }
        }

        public void RemoveItem(InventoryCell item, int quantity = 1) {
            var containedItemIndex = Array.FindIndex(container, cell => cell.item != null && cell == item);

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

        public bool HasSpace() => Array.FindAll(container, cell => cell.item != null).Length < maxLength;

        public InventoryCell[] FindItems(ItemObject item) {
            return Array.FindAll(container, cell => cell.item == item);
        }

        public bool HasItem(int itemId) =>
            Array.Find(container, cell => cell != null && cell.item.id == itemId) != null;
    }
}