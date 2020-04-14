using System.Collections.Generic;
using InventoryObjects.Items;
using UnityEngine;

namespace InventoryObjects.Inventory
{
    [CreateAssetMenu(fileName = "Create New InventoryController", menuName = "Inventory/InventoryController")]
    public class Inventory : ScriptableObject
    {
        [SerializeField] private int maxLength = 9;

        public List<InventoryCell> container = new List<InventoryCell>();
        
        
        public void AddItem(ItemObject item) {
            var containedItem = container.Find(cell => cell.item.id == item.id);
            if (containedItem != null) {
                containedItem.AddAmount(1);
            }
            else {
                if (container.Count < maxLength) {
                    container.Add(new InventoryCell(item, 1));
                }
            }
        }
        
        public void UpdateItems() {
            foreach (var cell in container) {
                cell.item.OnUpdate();
            }
        }

        public void RemoveItem(int itemId) {
            var containedItem = container.Find(cell => cell.item.id == itemId);
            if (containedItem == null) return;
            var amount = container[itemId].amount;
            if ( amount > 1) {
                containedItem.ReduceAmount(1);
            }
            else if (amount == 1) {
                container.Remove(containedItem);
            }
        }

        public void Clear() => container.Clear();
        public bool HasItem(int itemId) => container.Find(cell => cell.item.id == itemId) != null;
    }
}