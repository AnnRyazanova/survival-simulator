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
            _inventorySlots[i].Init(this, inventory.container[i]);
        }

        _weaponSlots[0].Init(this, new InventoryCell(equipment.weapon, 1));
        _weaponSlots[1].Init(this, new InventoryCell(equipment.tool, 1));
    }

    private void Init() {
        // TODO: Исправить копипасту
        foreach (var slot in _inventorySlots) {
            slot.Init(this, new InventoryCell(null, 0));
            slot.ThrowOut += SlotOnThrowOut;
            slot.ThrowOutAll += SlotOnThrowOutAll;
            slot.Use += SlotOnUse;
        }

        foreach (var slot in _weaponSlots) {
            slot.Init(this, new InventoryCell(null, 0));
            slot.ThrowOut += SlotOnThrowOut;
            slot.ThrowOutAll += SlotOnThrowOutAll;
            slot.Use += SlotOnUse;
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

    private void EquipItem(InventoryCell cell) {
        if (equipment.weapon == null) {
            equipment.weapon = (WeaponItem) cell.item;
            inventory.RemoveItem(cell.item.id);
        }
        else {
            if (equipment.weapon.id != cell.item.id) {
                var tmp = equipment.weapon;
                equipment.weapon = (WeaponItem) cell.item;
                inventory.RemoveItem(cell.item.id);
                inventory.AddItem(tmp);
            }
        }

        _weaponSlots[0].Init(this, new InventoryCell(equipment.weapon, 1));
        PlayerMainScript.MyPlayer.EquipWeapon();
    }

    private void UnequipItem(InventoryCell cell) {
        inventory.AddItem(cell.item);
        equipment.weapon = null;
    }

    private void SlotOnUse(InventoryCell inventoryCell) {
        switch (inventoryCell.item.ItemType) {
            case ItemObjectType.Consumable:
                inventoryCell.item.OnUse(PlayerMainScript.MyPlayer.playerObject);
                inventory.RemoveItem(inventoryCell.item.id);
                break;
            case ItemObjectType.Weapon:
            {
                EquipItem(inventoryCell);
                break;
            }
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