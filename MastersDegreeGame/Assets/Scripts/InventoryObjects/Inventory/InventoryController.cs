using System.Collections.Generic;
using InventoryObjects.Items;
using UnityEngine;

namespace InventoryObjects.Inventory
{
    [CreateAssetMenu(fileName = "Create New InventoryController", menuName = "Inventory/InventoryController")]
    public class InventoryController : ScriptableObject
    {
        [SerializeField] private int maxLength;

        private readonly Dictionary<int, InventoryCell> _container = new Dictionary<int, InventoryCell>();

        public void AddItem(ItemObject item) {
            var itemId = item.id;
            if (_container.ContainsKey(itemId)) {
                _container[itemId].AddAmount(1);
            }
            else {
                if (_container.Count < maxLength) {
                    _container[itemId] = new InventoryCell(item, 1);
                }
            }
        }

        public void UpdateItems() {
            foreach (var cell in _container) {
                cell.Value.item.OnUpdate();
            }
        }

        public void RemoveItem(int itemId) {
            if (!_container.ContainsKey(itemId)) return;
            var amount = _container[itemId].Amount;
            if ( amount > 1) {
                _container[itemId].ReduceAmount(1);
            }
            else if (amount == 1) {
                _container.Remove(itemId);
            }
        }

        public bool HasItem(int itemId) => _container.ContainsKey(itemId);
    }
}