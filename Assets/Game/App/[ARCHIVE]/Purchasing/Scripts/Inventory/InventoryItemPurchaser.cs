#if UNITY_PURCHASING
using System;
using Game.GameEngine.InventorySystem;
using Game.Meta;
using Purchasing;
using Services;

namespace Game.App
{
    public sealed class InventoryItemPurchaser
    {
        public event Action<InventoryItem> OnItemPurchased;

        public event Action<InventoryItem> OnItemPurchaseFailed;

        [ServiceInject]
        private PurchaseManager purchaseManager;

        public void Purchase(InventoryItem prototype)
        {
            var iapComponent = prototype.GetComponent<IComponent_IAPProduct>();
            var productId = iapComponent.ProductId;

            purchaseManager.Purchase(productId, result =>
            {
                if (result.isSuccess)
                {
                    AddItemInInventory(prototype);
                    OnItemPurchased?.Invoke(prototype);
                }
                else
                {
                    OnItemPurchaseFailed?.Invoke(prototype);
                }
            });
        }

        private void AddItemInInventory(InventoryItem prototype)
        {
            ServiceLocator
                .GetService<GameFacade>()
                .GetService<InventoryService>()
                .GetInventory()
                .AddItemsByPrototype(prototype, 1);
        }
    }
}
#endif