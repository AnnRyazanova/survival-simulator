using InventoryObjects.Items;
using UnityEngine;

namespace InventoryObjects.Inventory
{
    [CreateAssetMenu(fileName = "Create New Equipment", menuName = "Inventory/Equipment")]
    public class Equipment : ScriptableObject
    {
        public ItemObject weapon;
        public ItemObject tool;
    }
}
