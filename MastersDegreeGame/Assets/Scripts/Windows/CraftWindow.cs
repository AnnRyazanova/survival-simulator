using System;
using System.Collections;
using System.Collections.Generic;
using InventoryObjects.Crafting;
using InventoryObjects.Inventory;
using UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class CraftWindow : BaseWindow
{
    [SerializeField] private CraftSlot[] _craftSlots;
    [SerializeField] private CraftSlot[] _resourcesSlots;
    [Space(20)] [SerializeField] private Text _title;
    [SerializeField] private Text _description;
    [SerializeField] private Button _createButton;
    [SerializeField] private Inventory _inventory;

    [SerializeField] private CraftSlot _activeSlot;
    
    public override void Show() {
        base.Show();
        Init();
    }

    public void OnCreateBtnClick() {
        if (_activeSlot != null) {
            _activeSlot.recipe.CraftItem(_inventory);
            Init();
            SelectCraftableItem(_activeSlot);
        }
    }

    private void Awake() {
        _title.text = "";
        _description.text = "";
        _createButton.enabled = false;
    }

    private void Init() {
        foreach (var slot in _craftSlots) {
            slot.selectCraftableItem += SelectCraftableItem;
            if (slot.recipe != null) {
                if (slot.recipe.HasAllComponents(_inventory)) {
                    slot.SetFaderActive(false);
                }
                else {
                    slot.SetFaderActive(true);
                }
            }
        }
    }

    private void SelectCraftableItem(CraftSlot slot) {
        _activeSlot = slot;
        var hasAllIngredients = true;
        for (var i = 0; i < slot.recipe.ingredients.Count; i++) {
            var foundIngredient = slot.recipe.foundIngredients.Find(cell => slot.recipe.ingredients[i].item == cell.item) != null;
            _resourcesSlots[i].Init(slot.recipe.ingredients[i], !foundIngredient);
            hasAllIngredients &= foundIngredient;
        }

        _createButton.enabled = hasAllIngredients;
        _title.text = slot.recipe.result.item.title;
        _description.text = slot.recipe.result.item.description;
    }
}