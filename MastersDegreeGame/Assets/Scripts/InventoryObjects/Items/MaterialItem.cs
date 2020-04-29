using UnityEngine;
using Object = Characters.Object;

namespace InventoryObjects.Items
{
    [CreateAssetMenu(fileName = "Create New Material", menuName = "Inventory/Items/Material")]
    public class MaterialItem : ItemObject
    {
        private void Awake() {
            ItemType = ItemObjectType.Material;
            // Materials don't have durability decrease, they cannot be broken or stale
            durability = 100f;
            durabilityDecreaseRate = 0f;
        }

        public override void OnUse(Object target) {
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