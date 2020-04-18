using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Controllers;
using InventoryObjects.Inventory;
using InventoryObjects.Items;
using UI;
using UnityEngine;

public class InventoryWindow : BaseWindow
{
    [SerializeField] private InventorySlot[] _inventorySlots;
    [SerializeField] private InventorySlot[] _weaponSlots;
    
    public Inventory inventory;

    public void Awake() {
        Init();
    }

    public override void Show()
    {
        base.Show();
        Display();
    }

    private void Display() {
        for (var i = 0; i < inventory.maxLength; ++i) {
            _inventorySlots[i].Init(this, inventory.container[i]);
        }
    }

    private void Init()
    {
        foreach (var slot in _inventorySlots) {
            slot.Init(this, new InventoryCell(null, 0));
            slot.ThrowOut += SlotOnThrowOut;
            slot.ThrowOutAll += SlotOnThrowOutAll;
            slot.Use += SlotOnUse;
        }

        foreach (var slot in _weaponSlots) {
            slot.Init(this, new InventoryCell(null, 0));
        }
    }

    private void SlotOnThrowOut(InventoryCell inventoryCell) {
        inventory.RemoveItem(inventoryCell.item.id);
        Display();
    }
    
    private void SlotOnThrowOutAll(InventoryCell inventoryCell) {
        inventory.RemoveItem(inventoryCell.item.id, inventoryCell.amount);
        Display();
    }

    private void SlotOnUse(InventoryCell inventoryCell) {
        Debug.Log(inventoryCell.item.ItemType);
        switch (inventoryCell.item.ItemType) {
            case ItemObjectType.Consumable:
                inventoryCell.item.OnUse(PlayerMainScript.MyPlayer.playerObject);
                break;
            case ItemObjectType.Weapon:
                break;
            case ItemObjectType.Material:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        inventory.RemoveItem(inventoryCell.item.id);
        Display();
    }
}
