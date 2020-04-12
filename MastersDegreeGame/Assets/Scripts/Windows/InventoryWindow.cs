using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWindow : BaseWindow
{
    [SerializeField] private InventorySlot[] _inventorySlots;
    [SerializeField] private InventorySlot[] _weaponSlots;
    
    public override void Show()
    {
        base.Show();
        Init();
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
