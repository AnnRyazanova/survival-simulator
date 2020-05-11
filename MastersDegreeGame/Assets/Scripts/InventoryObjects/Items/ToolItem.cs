using System;
using Characters.Player;
using UnityEngine;
using Object = Characters.Object;

namespace InventoryObjects.Items
{
    [CreateAssetMenu(fileName = "Create New Tool", menuName = "Inventory/Items/Tool")]
    public class ToolItem : ItemObject
    {
        protected override void Awake() 
        {
            base.Awake();
            ItemType = ItemObjectType.Tool;
            isStackable = false;
        }
        
        public GameObject toolPrefab;
    }
}