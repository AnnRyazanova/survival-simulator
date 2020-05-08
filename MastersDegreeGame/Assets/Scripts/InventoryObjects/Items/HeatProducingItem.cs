using UnityEngine;

namespace InventoryObjects.Items
{
    [CreateAssetMenu(fileName = "Create New Heating", menuName = "Inventory/Items/Heat")]
    public class HeatProducingItem : ItemObject
    {
        public int heatRadius = 10;
        public int lightRadius = 10;

        public GameObject heatItemPrefab;
        
        public HeatProducingItem() {
            base.Awake();
            ItemType = ItemObjectType.Heat;
        }
    }
}
