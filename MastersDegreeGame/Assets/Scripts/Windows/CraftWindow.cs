using System;
using System.Collections;
using System.Collections.Generic;
using InventoryObjects.Crafting;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class CraftWindow : BaseWindow
{
    [SerializeField] private CraftSlot[] _craftSlots;
    [SerializeField] private CraftSlot[] _resourcesSlots;
    [Space(20)]
    
    [SerializeField] private Text _title;
    [SerializeField] private Text _description;
    
    public override void Show()
    {
        base.Show();
        Init();
    }

    public void OnCreateBtnClick()
    {
        Debug.Log("CreateBtn Click");
    }

    private void Awake()
    {
        _title.text = "";
        _description.text = "";
    }

    private void Init()
    {
        foreach (var slot in _craftSlots) {
            slot.selectCraftableItem += SelectCraftableItem;
        }
    }

    private void SelectCraftableItem(CraftingRecipe recipe) {
        for (var i = 0; i < recipe.ingredients.Count; i++) {
        }
    }
}
