﻿using InventoryObjects.Items;
using UnityEngine;

namespace InventoryObjects.Inventory
{
    [CreateAssetMenu(fileName = "Create New Equipment", menuName = "Inventory/Equipment")]
    public class Equipment : ScriptableObject
    {
        public InventoryCell weapon;
        public InventoryCell tool;
    }
}
