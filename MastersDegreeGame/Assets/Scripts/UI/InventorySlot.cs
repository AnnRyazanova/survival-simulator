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
    public sealed class InventorySlot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _count;
        [SerializeField] private GameObject popupPanel;

        private InventoryCell _cell;

        private InventoryWindow _window;

        // Popup window transform to stay on absolute foreground
        // regarding other slots
        private Transform _uiPopup;

        // Original parent to go to, after changing it for _uiPopup
        private Transform _originalParent;

        public event Action<InventoryCell> ThrowOut;
        public event Action<InventoryCell> ThrowOutAll;
        public event Action<InventoryCell> Use;

        public void Init(InventoryWindow window, InventoryCell inventoryCell) {
            _window = window;
            _cell = inventoryCell;
            if (_cell.item != null) {
                _icon.sprite = inventoryCell.item.displayIcon;
                _icon.gameObject.SetActive(true);
                if (_cell.item.ItemType == ItemObjectType.Weapon || _cell.item.ItemType == ItemObjectType.Tool) {
                    // GetComponents<Button>()[0].GetComponent<Text>().text = "НАДЕТЬ";
                }
            }
            else {
                _icon.gameObject.SetActive(false);
            }
            _count.SetText(_cell.amount > 1 ? _cell.amount.ToString() : "");
        }

        public void OnThrowOut() {
            Debug.Log("On throw " + _cell.item.id);
            ThrowOut?.Invoke(_cell);
            popupPanel.SetActive(false);
        }

        public void OnThrowOutAll() {
            ThrowOutAll?.Invoke(_cell);
            popupPanel.SetActive(false);
        }

        public void OnUse() {
            Debug.Log("On use " + _cell.item);
            Use?.Invoke(_cell);
            popupPanel.SetActive(false);
        }

        private void Awake() {
            _icon.gameObject.SetActive(false);
            popupPanel.SetActive(false);

            _uiPopup = GameObject.FindWithTag("UiPopUp").transform;
            _originalParent = transform.parent;
        }

        // Call a popup panel with actions
        public void OnPointerClick(PointerEventData eventData) {
            if (_window.popUpPanel != null && _window.popUpPanel != popupPanel) {
                _window.popUpPanel.SetActive(false);
            }
            if (_cell == null || _cell.item == null) return;
            popupPanel.SetActive(!popupPanel.activeSelf);
            if (popupPanel.activeSelf) {
                transform.SetParent(_uiPopup);
            }
            else {
                transform.SetParent(_originalParent);
            }
            _window.popUpPanel = popupPanel;
        }
    }
}