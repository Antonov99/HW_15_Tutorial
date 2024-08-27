using Game.GameEngine.Products;
using Game.Gameplay.Player;
using GameSystem;

namespace Game.Meta
{
    public sealed class ProductAnalyticsTrackerV1 :
        IGameReadyElement,
        IGameFinishElement
    {
        [GameInject]
        private ProductBuyer buyManager;

        [GameInject]
        private MoneyAnalyticsSupplier moneySupplier;

        void IGameReadyElement.ReadyGame()
        {
            buyManager.OnBuyCompleted += OnBuyProduct;
        }

        void IGameFinishElement.FinishGame()
        {
            buyManager.OnBuyCompleted -= OnBuyProduct;
        }

        private void OnBuyProduct(Product product)
        {
            ProductAnalytics.LogProductBought(
                product,
                previousMoney: moneySupplier.PreviousMoney,
                currentMoney: moneySupplier.CurrentMoney
            );
        }
    }
}