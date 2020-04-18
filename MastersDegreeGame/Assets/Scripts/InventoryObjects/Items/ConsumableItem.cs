using System;
using UnityEngine;

namespace InventoryObjects.Items
{
    [CreateAssetMenu(fileName = "Create New Consumable", menuName = "Inventory/Items/Consumable")]
    public class ConsumableItem : ItemObject
    {
        public int healthRegen = 2;
        public int hungerRegen = 10;
        
        private void Awake() {
            ItemType = ItemObjectType.Consumable;
        }

        public override void OnUse(Object target) {
            Debug.Log("Entered on use");
            if (target is PlayerObject playerObject) {
                playerObject.Health.AddPoints(healthRegen);
                playerObject.Hunger.AddPoints(hungerRegen);
            }
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