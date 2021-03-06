﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using InventoryObjects.Inventory;
using UnityEngine;

namespace InventoryObjects.Crafting
{
    [CreateAssetMenu(fileName = "Create New Crafting Recipe", menuName = "Inventory/Crafting/Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        public List<InventoryCell> ingredients = new List<InventoryCell>();
        public List<InventoryCell[]> foundIngredients = new List<InventoryCell[]>();
        public InventoryCell result = new InventoryCell(null, 0);
        public int canCraftCount = 0;
        
        /// <summary>
        /// Check and find all necessary components in player inventory and fill them inside local container
        /// </summary>
        /// <param name="inventory">Player inventory</param>
        /// <returns>If can craft an item</returns>
        public bool HasAllComponents(Inventory.Inventory inventory) {
            var presentIngredientsCounters = new int[ingredients.Count];
            foundIngredients.Clear();
            for (var i = 0; i < ingredients.Count; ++i) {
                var foundCells = inventory.FindItems(ingredients[i].item);

                if (foundCells.Length != 0) {
                    var count = 0;
                    foreach (var foundCell in foundCells) {
                        count += foundCell.amount;
                    }

                    if (count >= ingredients[i].amount) {
                        presentIngredientsCounters[i] = count / ingredients[i].amount;
                        Array.Sort(foundCells, (first, second) => second.CompareTo(first));
                        foundIngredients.Add(foundCells);
                    }
                }
            }

            result.amount = foundIngredients.Count == ingredients.Count ? presentIngredientsCounters.Min() : 0;
            return foundIngredients.Count == ingredients.Count;
        }


        /// <summary>
        /// Craft an item based on player inventory
        /// </summary>
        /// <param name="inventory">Player inventory</param>
        public void CraftItem(Inventory.Inventory inventory) {
            if (foundIngredients == null || foundIngredients.Count < ingredients.Count) return;

            for (var i = 0; i < ingredients.Count; ++i) {
                var ingredientsUsedCount = ingredients[i].amount;
                for (var j = 0; j < foundIngredients[i].Length; ++j) {
                    ingredientsUsedCount -= foundIngredients[i][j].amount;
                    if (ingredientsUsedCount <= 0) {
                        inventory.RemoveItem(foundIngredients[i][j], foundIngredients[i][j].amount + ingredientsUsedCount);
                        break;
                    }
                    inventory.RemoveItem(foundIngredients[i][j], foundIngredients[i][j].amount);
                }
            }

            var indexToAddTo = inventory.FindFreeCellToAdd(result.item);
            if (indexToAddTo != -1) {
                inventory.AddItem(result.item, indexToAddTo);
            }

            result.amount--;
        }
    }
}