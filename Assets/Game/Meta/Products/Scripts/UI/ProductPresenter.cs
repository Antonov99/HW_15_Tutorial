using System;
using Game.GameEngine.GameResources;
using Game.GameEngine.Mechanics;
using Game.GameEngine.Products;
using Game.Gameplay.Player;
using UnityEngine;

namespace Game.Meta
{
    public sealed class ProductPresenter
    {
        private readonly ProductView view;

        private ProductBuyer buyManager;

        private MoneyStorage moneyStorage;

        private ResourceStorage resourceStorage;

        private ResourceInfoCatalog resourceCatalog;

        private Sprite moneyIcon;

        private Product currentProduct;

        private bool isStarted;

        public ProductPresenter(ProductView view)
        {
            this.view = view;
        }

        public void Construct(
            ProductBuyer buyManager,
            MoneyStorage moneyStorage,
            ResourceStorage resourceStorage,
            ResourceInfoCatalog resourceCatalog,
            Sprite moneyIcon
        )
        {
            this.buyManager = buyManager;
            this.moneyStorage = moneyStorage;
            this.resourceStorage = resourceStorage;

            this.resourceCatalog = resourceCatalog;
            this.moneyIcon = moneyIcon;
        }

        public void SetProduct(Product product)
        {
            currentProduct = product;
            UpdateInfo();
            UpdatePrice();
            UpdateBuyButtonState();
        }

        public void Start()
        {
            if (isStarted)
            {
                return;
            }

            moneyStorage.OnMoneyChanged += OnMoneyChanged;
            resourceStorage.OnResourceChanged += OnResourcesChanged;
            buyManager.OnBuyCompleted += OnBuyCompleted;
            
            view.BuyButton.OnClicked += OnBuyButtonClicked;
            isStarted = true;
            
            UpdateBuyButtonState();
        }
        
        public void Stop()
        {
            if (!isStarted)
            {
                return;
            }

            buyManager.OnBuyCompleted -= OnBuyCompleted;
            view.BuyButton.OnClicked -= OnBuyButtonClicked;
            moneyStorage.OnMoneyChanged -= OnMoneyChanged;
            resourceStorage.OnResourceChanged -= OnResourcesChanged;
            
            isStarted = false;
        }

        #region GameEvents

        private void OnBuyCompleted(Product product)
        {
            UpdateBuyButtonState();
        }

        private void OnResourcesChanged(ResourceType type, int amount)
        {
            UpdateBuyButtonState();
        }

        private void OnMoneyChanged(int money)
        {
            UpdateBuyButtonState();
        }

        #endregion

        #region UIEvents

        private void OnBuyButtonClicked()
        {
            if (buyManager.CanBuyProduct(currentProduct))
            {
                buyManager.BuyProduct(currentProduct);
            }
        }

        #endregion

        private void UpdateInfo()
        {
            var metadata = currentProduct.Metadata;
            view.SetTitle(metadata.Title);
            view.SetDescription(metadata.Decription);
            view.SetIcon(metadata.Icon);
        }

        private void UpdateBuyButtonState()
        {
            var canBuyProduct = buyManager.CanBuyProduct(currentProduct);
            var state = canBuyProduct
                ? ProductBuyButton.State.AVAILABLE
                : ProductBuyButton.State.LOCKED;
            view.BuyButton.SetState(state);
        }

        private void UpdatePrice()
        {
            if (currentProduct.TryGetComponent(out Component_MoneyPrice moneyPriceComponent))
            {
                SetMoneyPrice(moneyPriceComponent);
            }
            else if (currentProduct.TryGetComponent(out Component_ResourcePrice resourcePriceComponent))
            {
                SetResourcePrice(resourcePriceComponent);
            }
        }

        private void SetMoneyPrice(Component_MoneyPrice component)
        {
            var buyButton = view.BuyButton;
            buyButton.SetPriceSize1();

            var pricePanel = buyButton.PriceItem1;
            pricePanel.SetIcon(moneyIcon);
            pricePanel.SetPrice(component.Price.ToString());
        }

        private void SetResourcePrice(Component_ResourcePrice component)
        {
            var buyButton = view.BuyButton;
            var resources = component.GetPrice();
            var length = resources.Length;
            if (length is < 1 or > 2)
            {
                throw new Exception("Button support only 1 or 2 price items!");
            }

            var resource1 = resources[0];
            var pricePanel1 = buyButton.PriceItem1;
            pricePanel1.SetPrice(resource1.amount.ToString());
            pricePanel1.SetIcon(resourceCatalog.FindResource(resource1.type).icon);

            if (length == 1)
            {
                buyButton.SetPriceSize1();
                return;
            }

            var resource2 = resources[1];
            var pricePanel2 = buyButton.PriceItem2;
            pricePanel2.SetPrice(resource2.amount.ToString());
            pricePanel2.SetIcon(resourceCatalog.FindResource(resource2.type).icon);

            buyButton.SetPriceSize2();
        }
    }
}