using System.Collections.Generic;
using System.Threading;
using InventoryObjects.Inventory;
using UnityEngine;

namespace InventoryObjects.Crafting
{
    [CreateAssetMenu(fileName = "Create New Crafting Recipe", menuName = "Inventory/Crafting/Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        public List<InventoryCell> ingredients = new List<InventoryCell>();
        public List<InventoryCell> foundIngredients = new List<InventoryCell>();
        public InventoryCell result = new InventoryCell(null, 0);

        /// <summary>
        /// Check and find all necessary components in player inventory and fill them inside local container
        /// </summary>
        /// <param name="inventory">Player inventory</param>
        /// <returns>If can craft an item</returns>
        public bool HasAllComponents(Inventory.Inventory inventory) {
            foundIngredients.Clear();
            foreach (var ingredient in ingredients) {
                var foundCell = inventory.FindItem(ingredient.item);
                if (foundCell == null || foundCell.amount < ingredient.amount) {
                    return false;
                }
                foundIngredients.Add(foundCell);
            }
            return foundIngredients.Count <= ingredients.Count;
        }

        /// <summary>
        /// Craft an item based on player inventory
        /// </summary>
        /// <param name="inventory">Player inventory</param>
        public void CraftItem(Inventory.Inventory inventory) {
            if (foundIngredients == null || foundIngredients.Count < ingredients.Count) return;
            
            for (var i = 0; i < ingredients.Count; ++i) {
                inventory.RemoveItem(foundIngredients[i].item, ingredients[i].amount);
            }

            var indexToAddTo = inventory.FindFreeCellToAdd(result.item);
            if (indexToAddTo != -1) {
                inventory.AddItem(result.item, indexToAddTo, result.amount);
            }
        }
    }
}