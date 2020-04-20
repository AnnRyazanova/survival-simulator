using Characters.Controllers;
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
        _weaponSlots[0].Init(this, new InventoryCell(equipment.weapon, 1));
        _weaponSlots[1].Init(this, new InventoryCell(equipment.tool, 1));
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

    private void EquipSlot(ref ItemObject equipmentObject, InventoryCell inventoryCell) {
        Debug.Log("On equip");
        if (equipmentObject == null) {
            equipmentObject = inventoryCell.item;
        }
        else {
            if (equipmentObject.id != inventoryCell.item.id) {
                var tmp = equipmentObject;
                equipmentObject = (WeaponItem) inventoryCell.item;
                inventory.AddItem(tmp);
            }
        }
    }

    private void SlotOnEquip(InventoryCell inventoryCell) {
        if (inventoryCell.item.ItemType == ItemObjectType.Weapon) {
            EquipSlot(ref equipment.weapon, inventoryCell);
            _weaponSlots[0].Init(this, new InventoryCell(equipment.weapon, 1));
            PlayerMainScript.MyPlayer.EquipWeapon();
        }
        else {
            EquipSlot(ref equipment.tool, inventoryCell);
            _weaponSlots[1].Init(this, new InventoryCell(equipment.tool, 1));
            PlayerMainScript.MyPlayer.EquipTool();
        }

        inventory.RemoveItem(inventoryCell);
        Display();
    }

    private void SlotOnUnequip(InventoryCell inventoryCell) {
        inventory.AddItem(inventoryCell.item);
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
            inventoryCell.item.OnUse(PlayerMainScript.MyPlayer.playerObject);
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