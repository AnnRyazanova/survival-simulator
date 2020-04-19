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
        for (var i = 0; i < inventory.maxLength; ++i) {
            _inventorySlots[i].Init(this, inventory.container[i], InventorySlotType.InventorySlot);
        }

        _weaponSlots[0].Init(this, new InventoryCell(equipment.weapon, 1), _weaponSlots[0].slotType);
        _weaponSlots[1].Init(this, new InventoryCell(equipment.tool, 1), _weaponSlots[0].slotType);
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
            slot.Init(this, new InventoryCell(null, 0), InventorySlotType.InventorySlot);
            SubscribeToCommonItemEvents(slot);
        }

        foreach (var slot in _weaponSlots) {
            slot.Init(this, new InventoryCell(null, 0), InventorySlotType.EquipmentSlot);
            SubscribeToCommonItemEvents(slot);
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

    private void SlotOnEquip(InventoryCell inventoryCell) {
        if (equipment.weapon == null) {
            equipment.weapon = (WeaponItem) inventoryCell.item;
            inventory.RemoveItem(inventoryCell.item.id);
        }
        else {
            if (equipment.weapon.id != inventoryCell.item.id) {
                var tmp = equipment.weapon;
                equipment.weapon = (WeaponItem) inventoryCell.item;
                inventory.RemoveItem(inventoryCell.item.id);
                inventory.AddItem(tmp);
            }
        }

        _weaponSlots[0].Init(this, new InventoryCell(equipment.weapon, 1), InventorySlotType.EquipmentSlot);
        PlayerMainScript.MyPlayer.EquipWeapon();
        inventory.RemoveItem(inventoryCell.item.id);
        Display();
    }

    private void SlotOnUnequip(InventoryCell cell) {
        inventory.AddItem(cell.item);
        equipment.weapon = null;
        PlayerMainScript.MyPlayer.UnequipWeapon();
        Display();
    }

    private void SlotOnUse(InventoryCell inventoryCell) {
        if (inventoryCell.item.ItemType == ItemObjectType.Consumable) {
            inventoryCell.item.OnUse(PlayerMainScript.MyPlayer.playerObject);
            inventory.RemoveItem(inventoryCell.item.id);
        }
        Display();
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (popUpPanel != null && popUpPanel.activeSelf) {
            Debug.Log("On InvWind");
            popUpPanel.SetActive(false);
        }
    }
}