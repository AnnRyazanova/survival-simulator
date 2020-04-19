using System;
using UnityEngine;

namespace InventoryObjects.Items
{
    [CreateAssetMenu(fileName = "Create New Tool", menuName = "Inventory/Items/Tool")]
    public class ToolItem : ItemObject
    {
        public GameObject toolPrefab;
        private void Awake() {
            ItemType = ItemObjectType.Tool;
            isStackable = false;
        }

        public override void OnUse(Object target) {
            Debug.Log("Entered on use " + ItemType);
            if (target is PlayerObject playerObject) {

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