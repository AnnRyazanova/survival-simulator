using System;
using System.Collections;
using System.Collections.Generic;
using InventoryObjects.Inventory;
using UnityEngine;

public class InventoryWindow : BaseWindow
{
    [SerializeField] private InventorySlot[] _inventorySlots;
    [SerializeField] private InventorySlot[] _weaponSlots;

    public Inventory inventory;

    public override void Show()
    {
        base.Show();
        Init();
        Display();
    }

    public void Display() {
        var itemIndex = 0;
        foreach (var item in inventory.container) {
            _inventorySlots[itemIndex].Init(this, item.item.displayIcon);
            itemIndex++;
        }
    }
    
    private void Init()
    {
        foreach (var slot in _inventorySlots) {
            slot.Init(this);
        }

        foreach (var slot in _weaponSlots) {
            slot.Init(this);
        }
    }
}
