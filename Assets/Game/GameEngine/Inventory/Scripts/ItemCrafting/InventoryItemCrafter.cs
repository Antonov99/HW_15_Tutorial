using System;
using Sirenix.OdinInspector;

namespace Game.GameEngine.InventorySystem
{
    public class InventoryItemCrafter
    {
        public event Action<InventoryItemReceipt> OnCraftFinished;

        private StackableInventory inventory;

        public InventoryItemCrafter()
        {
        }

        public InventoryItemCrafter(StackableInventory inventory)
        {
            this.inventory = inventory;
        }

        public void SetInventory(StackableInventory inventory)
        {
            this.inventory = inventory;
        }

        [Button]
        [GUIColor(0, 1, 0)]
        public bool CanCraftItem(InventoryItemReceipt receipt)
        {
            var ingredients = receipt.ingredients;
            for (int i = 0, count = ingredients.Length; i < count; i++)
            {
                var ingredient = ingredients[i];
                if (!IngredientExists(ingredient))
                {
                    return false;
                }
            }

            return true;
        }

        [Button]
        [GUIColor(0, 1, 0)]
        public void CraftItem(InventoryItemReceipt receipt)
        {
            var ingredients = receipt.ingredients;
            for (int i = 0, count = ingredients.Length; i < count; i++)
            {
                var ingredient = ingredients[i];
                ConsumeIngredient(ingredient);
            }

            ProduceResult(receipt);
            OnCraftFinished?.Invoke(receipt);
        }

        private bool IngredientExists(InventoryItemIngredient ingredient)
        {
            var count = ingredient.requiredCount;
            var itemName = ingredient.itemInfo.ItemName;
            return inventory.CountItems(itemName) >= count;
        }

        private void ConsumeIngredient(InventoryItemIngredient ingredient)
        {
            var itemName = ingredient.itemInfo.ItemName;
            var count = ingredient.requiredCount;
            inventory.RemoveItems(itemName, count);
        }

        private void ProduceResult(InventoryItemReceipt receipt)
        {
            var resultItem = receipt.resultInfo.Prototype;
            inventory.AddItemsByPrototype(resultItem, 1);
        }
    }
}