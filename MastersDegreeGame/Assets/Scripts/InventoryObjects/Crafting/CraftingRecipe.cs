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
        public InventoryCell result = new InventoryCell(null, 0);

        /// <summary>
        /// Check and find all necessary components in player inventory
        /// </summary>
        /// <param name="inventory">Player inventory</param>
        /// <returns>List of all ingredients found in inventory</returns>
        public List<InventoryCell> CheckForComponents(Inventory.Inventory inventory) {
            var foundIngredients = new List<InventoryCell>();
            foreach (var ingredient in ingredients) {
                var foundCell = inventory.FindItem(ingredient.item);
                if (foundCell.amount >= ingredient.amount) {
                    foundIngredients.Add(foundCell);
                }
            }

            return foundIngredients;
        }
        
        /// <summary>
        /// Craft an item based on player inventory
        /// </summary>
        /// <param name="inventory">Player inventory</param>
        public void CraftItem(Inventory.Inventory inventory) {
            var foundIngredients = CheckForComponents(inventory);
            if (foundIngredients.Count < ingredients.Count) {
                return;
            }

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