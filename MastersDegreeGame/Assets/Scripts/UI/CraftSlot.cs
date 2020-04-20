using System;
using System.Collections;
using System.Collections.Generic;
using InventoryObjects.Crafting;
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
        [SerializeField] private CraftingRecipe recipe = null;

        public Action<CraftingRecipe> selectCraftableItem;

        public void OnSelectCraftableItem() {
            selectCraftableItem?.Invoke(recipe);    
        }
        
        private void Awake()
        {
            _icon.gameObject.SetActive(false);
            _count.gameObject.SetActive(false);
            _fader.SetActive(false);
            
            Debug.Log("On awake");
            if (recipe.result.item != null) {
                Debug.Log("On not item null");
                _icon.sprite = recipe.result.item.displayIcon;
                _count.text = recipe.result.amount.ToString();
                _icon.gameObject.SetActive(true);
                _count.gameObject.SetActive(true);
            }
        }
        
        
        public void OnPointerClick(PointerEventData eventData) {
            if (recipe != null) {
                OnSelectCraftableItem();
            }
        }
    }
}
