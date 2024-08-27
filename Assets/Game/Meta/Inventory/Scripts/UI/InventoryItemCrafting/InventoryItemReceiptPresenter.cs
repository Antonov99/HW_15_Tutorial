using Game.GameEngine.InventorySystem;

namespace Game.Meta
{
    public sealed class InventoryItemReceiptPresenter
    {
        private readonly InventoryItemReceiptView view;

        private readonly InventoryItemReceipt receipt;

        private InventoryItemCrafter craftManager;

        private StackableInventory inventory;

        public InventoryItemReceiptPresenter(InventoryItemReceiptView view, InventoryItemReceipt receipt)
        {
            this.view = view;
            this.receipt = receipt;
        }

        public void Construct(InventoryItemCrafter craftManager, StackableInventory inventory)
        {
            this.craftManager = craftManager;
            this.inventory = inventory;
        }

        public void Start()
        {
            SetupIngredients();
            SetupResult();
            SetupCraftButton();
            view.OnCraftButtonClicked += OnCraftButtonClicked;
        }

        private void SetupCraftButton()
        {
            var canCraft = craftManager.CanCraftItem(receipt);
            view.SetInteractibleButton(canCraft);
        }

        public void Stop()
        {
            view.OnCraftButtonClicked -= OnCraftButtonClicked;
        }

        private void OnCraftButtonClicked()
        {
            if (!craftManager.CanCraftItem(receipt))
            {
                return;
            }

            craftManager.CraftItem(receipt);

            //TODO: Show particles
                
            //Update state:
            SetupIngredients();
            SetupCraftButton();
        }

        private void SetupResult()
        {
            var resultItem = receipt.resultInfo;
            var metadata = resultItem.Metadata;
            view.SetTitle(metadata.title);
            view.SetDescription(metadata.decription);
            view.SetIcon(metadata.icon);
        }

        private void SetupIngredients()
        {
            var ingredients = receipt.ingredients;
            var ingredientCount = ingredients.Length;

            //Show used ingredient views:
            for (var i = 0; i < ingredientCount; i++)
            {
                var ingredient = ingredients[i];
                var ingredientView = view.GetIngredient(i);
                var ingredientItem = ingredient.itemInfo;

                var title = ingredientItem.Metadata.title;
                var requiredCount = ingredient.requiredCount;
                var actualCount = inventory.CountItems(ingredientItem.ItemName);
                ingredientView.Setup(title, requiredCount, actualCount);
                ingredientView.SetVisible(true);
            }

            //Hide unused ingredients views:
            for (int i = ingredientCount, end = view.IngredientCount; i < end; i++)
            {
                var ingredientView = view.GetIngredient(i);
                ingredientView.SetVisible(false);
            }
        }
    }
}