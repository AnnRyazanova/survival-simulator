﻿using System;
using InventoryObjects.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

        public void Init(InventoryWindow window, InventoryCell inventoryCell = null) {
            _window = window;
            _cell = inventoryCell;
            if (_cell != null && _cell.item != null) {
                _icon.sprite = inventoryCell.item.displayIcon;
                _icon.gameObject.SetActive(true);
                _count.SetText(_cell.amount > 1 ? _cell.amount.ToString() : "");
                return;
            }
            _icon.gameObject.SetActive(false);
        }

        public void OnThrowOut() {
            Debug.Log("On throw " + _cell.item.id);
            popupPanel.SetActive(false);
            ThrowOut?.Invoke(_cell);
        }

        public void OnThrowOutAll() {
            popupPanel.SetActive(false);
            ThrowOutAll?.Invoke(_cell);
        }

        public void OnUse() {
        }

        private void Awake() {
            _icon.gameObject.SetActive(false);
            popupPanel.SetActive(false);
            
            _uiPopup = GameObject.FindWithTag("UiPopUp").transform;
            _originalParent = transform.parent;
        }
        
        // Call a popup panel with actions
        public void OnPointerClick(PointerEventData eventData) {
            if (_cell == null || _cell.item == null) return;
            popupPanel.SetActive(!popupPanel.activeSelf);
            if (popupPanel.activeSelf) {
                transform.SetParent(_uiPopup);
            }
            else {
                transform.SetParent(_originalParent);
            }
        }
    }
}