using System;
using System.Collections;
using System.Collections.Generic;
using InventoryObjects.Crafting;
using InventoryObjects.Inventory;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class CraftWindow : BaseWindow
{
    [SerializeField] private CraftSlot[] _craftSlots;
    [SerializeField] private CraftSlot[] _resourcesSlots;
    [Space(20)] [SerializeField] private Text _title;
    [SerializeField] private Text _description;
    [SerializeField] private Button _createButton;
    [SerializeField] private Inventory _inventory;

    public override void Show() {
        base.Show();
        Init();
    }

    public void OnCreateBtnClick() {
        Debug.Log("CreateBtn Click");
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
                    _createButton.enabled = true;
                }
                else {
                    slot.SetFaderActive(true);
                }
            }
        }
    }


    private void SelectCraftableItem(CraftingRecipe recipe) {
        for (var i = 0; i < recipe.ingredients.Count; i++) {
            _resourcesSlots[i].Init(recipe.ingredients[i]);
        }

        _title.text = recipe.result.item.title;
        _description.text = recipe.result.item.description;
    }
}