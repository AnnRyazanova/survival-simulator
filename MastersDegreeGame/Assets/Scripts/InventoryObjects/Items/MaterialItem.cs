using UnityEngine;
using Object = Characters.Object;

namespace InventoryObjects.Items
{
    [CreateAssetMenu(fileName = "Create New Material", menuName = "Inventory/Items/Material")]
    public class MaterialItem : ItemObject
    {
        private void Awake() {
            base.Awake();
                ItemType = ItemObjectType.Material;
            // Materials don't have durability decrease, they cannot be broken or stale
        }
    }
}