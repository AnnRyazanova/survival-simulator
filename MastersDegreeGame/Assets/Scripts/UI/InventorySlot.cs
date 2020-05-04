using System;
using InventoryObjects.Inventory;
using InventoryObjects.Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace UI
{
    public enum InventorySlotType
    {
        Equipment,
        Inventory,
        PrevSet
    }

    public sealed class InventorySlot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _durability;
        [SerializeField] private TextMeshProUGUI _count;
        [SerializeField] private GameObject popupPanel;
        [SerializeField] private Text actionButtonText;
        [SerializeField] private Button[] actionButton;

        public InventoryCell _cell;

        private InventoryWindow _window;

        // Popup window transform to stay on absolute foreground
        // regarding other slots
        private Transform _uiPopup;

        // Original parent to go to, after changing it for _uiPopup
        private Transform _originalParent;

        public InventorySlotType slotType;

        #region ActionEvents

        public event Action<InventoryCell> ThrowOut;
        public event Action<InventoryCell> ThrowOutAll;
        public event Action<InventoryCell> Use;
        public event Action<InventoryCell> Equip;
        public event Action<InventoryCell> Unquip;

        #endregion

        private void Awake() {
            _icon.gameObject.SetActive(false);
            _durability.gameObject.SetActive(false);

            popupPanel.SetActive(false);
            _uiPopup = GameObject.FindWithTag("UiPopUp").transform;
            _originalParent = transform.parent;
        }

        public void Init(InventoryWindow window, InventoryCell inventoryCell, 
                                InventorySlotType _slotType = InventorySlotType.PrevSet) {
            _window = window;
            _cell = inventoryCell;
            slotType = _slotType == InventorySlotType.PrevSet ? slotType : _slotType;
            actionButton[0].gameObject.SetActive(true);
            SetupFromCell();
            if (_cell != null) {
                _count.gameObject.SetActive(true);
                _count.SetText(_cell.amount > 1 ? _cell.amount.ToString() : "");
            }
            else {
                _count.gameObject.SetActive(false);
            }
        }

        private void SetupFromCell() {
            if (_cell != null && _cell.item != null) {
                _icon.sprite =  _cell.item.displayIcon;
                _icon.gameObject.SetActive(true);
                _durability.gameObject.SetActive(true);
                
                _durability.fillAmount = _cell.NormalizedCurrentDurability;
                switch (_cell.item.ItemType) {
                    case ItemObjectType.Weapon:
                    case ItemObjectType.Tool:
                    {
                        if (slotType == InventorySlotType.Inventory) {
                            actionButtonText.text = "НАДЕТЬ";
                            for (var i = 1; i < actionButton.Length; ++i) {
                                actionButton[i].gameObject.SetActive(true);
                            }
                        }
                        else {
                            actionButtonText.text = "СНЯТЬ";
                            for (var i = 1; i < actionButton.Length; ++i) {
                                actionButton[i].gameObject.SetActive(false);
                            }
                        }

                        break;
                    }
                    case ItemObjectType.Material:
                        actionButton[0].gameObject.SetActive(false);
                        _durability.gameObject.SetActive(false);
                        break;
                    default:
                        actionButtonText.text = "ИСПОЛЬЗОВАТЬ";
                        break;
                }
            }
            else {
                _icon.gameObject.SetActive(false);
                _durability.gameObject.SetActive(false);
            }
        }

        public void OnThrowOut() {
            ThrowOut?.Invoke(_cell);
            popupPanel.SetActive(false);
        }

        public void OnThrowOutAll() {
            ThrowOutAll?.Invoke(_cell);
            popupPanel.SetActive(false);
        }

        public void OnActionButtonClick() {
            if (_cell.item.ItemType == ItemObjectType.Consumable) {
                Use?.Invoke(_cell);
            }
            else {
                if (slotType == InventorySlotType.Inventory) {
                    Equip?.Invoke(_cell);
                }
                else {
                    Unquip?.Invoke(_cell);
                }
            }

            popupPanel.SetActive(false);
        }

        // Call a popup panel with actions
        public void OnPointerClick(PointerEventData eventData) {
            if (_window.popUpPanel != null && _window.popUpPanel != popupPanel) {
                _window.popUpPanel.SetActive(false);
            }

            if (_cell == null || _cell.item == null) return;
            popupPanel.SetActive(!popupPanel.activeSelf);
            popupPanel.transform.SetParent(popupPanel.activeSelf ? _uiPopup : _originalParent);
            _window.popUpPanel = popupPanel;
        }
    }
}