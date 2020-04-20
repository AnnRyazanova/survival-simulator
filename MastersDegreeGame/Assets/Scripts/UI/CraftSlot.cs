using System;
using System.Collections;
using System.Collections.Generic;
using InventoryObjects.Crafting;
using InventoryObjects.Inventory;
using InventoryObjects.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class CraftSlot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _count;
        [SerializeField] private GameObject _fader;
        [SerializeField] private InventoryCell cell;
        
        public CraftingRecipe recipe;
        public Action<CraftingRecipe> selectCraftableItem;

        public void OnSelectCraftableItem() {
            selectCraftableItem?.Invoke(recipe);
        }

        private void Awake() {
            _icon.gameObject.SetActive(false);
            _count.gameObject.SetActive(false);
            _fader.SetActive(false);
            Init();
        }

        public void Init(InventoryCell newCell = null) {
            if (newCell == null) {
                if(recipe == null) return;
                cell = recipe.result;
            }
            else {
                cell = newCell;
            }
            
            _icon.sprite = cell.item.displayIcon;
            _count.text = cell.amount.ToString();
            
            _icon.gameObject.SetActive(true);
            _count.gameObject.SetActive(true);
        }

        public void SetFaderActive(bool state) {
            _fader.SetActive(state);
        }
        
        public void OnPointerClick(PointerEventData eventData) {
            if (recipe != null) {
                OnSelectCraftableItem();
            }
        }
    }
}