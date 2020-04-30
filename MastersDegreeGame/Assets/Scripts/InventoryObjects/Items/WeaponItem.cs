using UnityEngine;
using Object = Characters.Object;

namespace InventoryObjects.Items
{
    // Weapon items can be used as tools
    [CreateAssetMenu(fileName = "Create New Weapon", menuName = "Inventory/Items/Weapon")]
    public class WeaponItem : ItemObject
    {
        public int attackPower;

        public GameObject weaponPrefab;
        
        private void Awake() {
            base.Awake();
            ItemType = ItemObjectType.Weapon;
            isStackable = false;
        }
    }
}
