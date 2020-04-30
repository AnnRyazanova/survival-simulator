using Characters.Controllers;
using Characters.Player;
using InventoryObjects.Inventory;
using InventoryObjects.Items;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryWindow : BaseWindow, IPointerClickHandler
{
    [SerializeField] private InventorySlot[] _inventorySlots;
    [SerializeField] private InventorySlot[] _weaponSlots;
    [SerializeField] private Text useButtonText;

    [SerializeField] private Inventory inventory;
    [SerializeField] private Equipment equipment;

    public GameObject popUpPanel = null;

    public void Awake() {
        Init();
    }

    public override void Show() {
        base.Show();
        Display();
    }

    private void Display() {
        inventory.TidyLayout();

        for (var i = 0; i < inventory.maxLength; ++i) {
            _inventorySlots[i].Init(this, inventory.container[i]);
        }

        // TODO: Fix new to maybe some preallocated value
        _weaponSlots[0].Init(this, equipment.weapon);
        _weaponSlots[1].Init(this, equipment.tool);
    }

    private void SubscribeToCommonItemEvents(InventorySlot slot) {
        slot.ThrowOut += SlotOnThrowOut;
        slot.ThrowOutAll += SlotOnThrowOutAll;
        slot.Use += SlotOnUse;
        slot.Equip += SlotOnEquip;
        slot.Unquip += SlotOnUnequip;
    }

    private void Init() {
        foreach (var slot in _inventorySlots) {
                slot.Init(this, new InventoryCell(null, 0), InventorySlotType.Inventory);
                SubscribeToCommonItemEvents(slot);
        }

        foreach (var slot in _weaponSlots) {
                slot.Init(this, new InventoryCell(null, 0), InventorySlotType.Equipment);
                SubscribeToCommonItemEvents(slot);
        }
    }

    private void SlotOnThrowOut(InventoryCell inventoryCell) {
        inventory.RemoveItem(inventoryCell);
        Display();
    }

    private void SlotOnThrowOutAll(InventoryCell inventoryCell) {
        inventory.RemoveItem(inventoryCell, inventoryCell.amount);
        Display();
    }
    
    private void EquipSlot(ref InventoryCell equipmentObject, InventoryCell inventoryCell) {
        Debug.Log("On equip");
        if (equipmentObject == null || equipmentObject.item == null) {
            equipmentObject = inventoryCell;
        }
        else {
            if (equipmentObject.item.id != inventoryCell.item.id) {
                var tmp = equipmentObject;
                equipmentObject = inventoryCell;
                inventory.AddItem(tmp.item);
            }
        }
    }

    private void SlotOnEquip(InventoryCell inventoryCell) {
        if (inventoryCell.item.ItemType == ItemObjectType.Weapon) {
            EquipSlot(ref equipment.weapon, inventoryCell);
            _weaponSlots[0].Init(this, equipment.weapon);

            PlayerMainScript.MyPlayer.EquipWeapon();
        }
        else {
            EquipSlot(ref equipment.tool, inventoryCell);
            _weaponSlots[1].Init(this, equipment.tool);
            PlayerMainScript.MyPlayer.EquipTool();
        }

        inventory.RemoveItem(inventoryCell);
        Display();
    }

    private void SlotOnUnequip(InventoryCell inventoryCell) {
        inventory.AddCell(inventoryCell);
        if (inventoryCell.item.ItemType == ItemObjectType.Weapon) {
            PlayerMainScript.MyPlayer.UnequipWeapon();
            equipment.weapon = null;
        }
        else {
            PlayerMainScript.MyPlayer.UnequipTool();
            equipment.tool = null;
        }

        Display();
    }

    private void SlotOnUse(InventoryCell inventoryCell) {
        if (inventoryCell.item.ItemType == ItemObjectType.Consumable) {
            inventoryCell.Use(PlayerMainScript.MyPlayer.playerObject);
            inventory.RemoveItem(inventoryCell);
        }

        Display();
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (popUpPanel != null && popUpPanel.activeSelf) {
            popUpPanel.SetActive(false);
        }
    }
}