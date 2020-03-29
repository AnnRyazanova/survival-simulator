using System;
using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "Create New Consumable", menuName = "Inventory/Items/Consumable")]
    public class ConsumableItem : ItemObject
    {
        public float healthRegen;
        
        private void Awake() {
            ItemType = ItemObjectType.Consumable;
        }

        public override void OnUse(GameObject target) {
            throw new System.NotImplementedException();
        }

        public override void OnThrowOut() {
            throw new System.NotImplementedException();
        }

        public override void OnPickUp() {
            throw new System.NotImplementedException();
        }

        public override void OnUpdate() {
            throw new System.NotImplementedException();
        }
    }
}