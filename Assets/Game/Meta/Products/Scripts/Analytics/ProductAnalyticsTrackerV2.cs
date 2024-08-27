using Game.GameEngine.Products;
using Game.Gameplay.Player;
using GameSystem;
using UnityEngine;

namespace Game.Meta
{
    public sealed class ProductAnalyticsTrackerV2 : MonoBehaviour, 
        IGameReadyElement,
        IGameFinishElement
    {
        [GameInject]
        private ProductBuyer buyManager;

        [GameInject]
        private MoneyStorage moneyStorage;

        private int previousMoney;

        void IGameReadyElement.ReadyGame()
        {
            buyManager.OnBuyStarted += OnStartBuy;
            buyManager.OnBuyCompleted += OnFinishBuy;
        }

        void IGameFinishElement.FinishGame()
        {
            buyManager.OnBuyStarted -= OnStartBuy;
            buyManager.OnBuyCompleted -= OnFinishBuy;
        }

        private void OnStartBuy(Product product)
        {
            previousMoney = moneyStorage.Money;
        }

        private void OnFinishBuy(Product product)
        {
            ProductAnalytics.LogProductBought(product, previousMoney, moneyStorage.Money);
        }
    }
}