using System.Collections.Generic;
using Game.GameEngine.GameResources;
using Game.GameEngine.Products;
using Game.Gameplay.Player;
using Game.UI;
using GameSystem;
using Windows;
using UnityEngine;

namespace Game.Meta
{
    public sealed class ProductListPresenterOptimized : MonoWindow,
        IGameConstructElement,
        RecyclableListView.IAdapter
    {
        [SerializeField]
        private RecyclableListView recycleViewList;

        [Space]
        [SerializeField]
        private ProductCatalog productCatalog;

        [SerializeField]
        private ResourceInfoCatalog resourceCatalog;

        [SerializeField]
        private Sprite moneyIcon;

        private readonly Dictionary<RectTransform, ProductPresenter> presenterMap = new();

        private ProductBuyer buyManager;

        private MoneyStorage moneyStorage;

        private ResourceStorage resourceStorage;

        protected override void OnShow(object args)
        {
            recycleViewList.Initialize(adapter: this);
        }

        protected override void OnHide()
        {
            recycleViewList.Terminate();
        }

        int RecyclableListView.IAdapter.GetDataCount()
        {
            return productCatalog.ProductCount;
        }

        void RecyclableListView.IAdapter.OnCreateView(RectTransform view, int index)
        {
            var viewComponent = view.GetComponent<ProductView>();
            var presenter = new ProductPresenter(viewComponent);
            presenter.Construct(
                buyManager,
                moneyStorage,
                resourceStorage,
                resourceCatalog,
                moneyIcon
            );
            presenterMap.Add(view, presenter);

            var productConfig = productCatalog.GetProduct(index);
            presenter.SetProduct(productConfig.Prototype);
            presenter.Start();
        }

        void RecyclableListView.IAdapter.OnUpdateView(RectTransform view, int index)
        {
            var presenter = presenterMap[view];
            var productConfig = productCatalog.GetProduct(index);
            presenter.SetProduct(productConfig.Prototype);
        }

        void RecyclableListView.IAdapter.OnDestroyView(RectTransform view)
        {
            var presenter = presenterMap[view];
            presenter.Stop();
            presenterMap.Remove(view);
        }

        void IGameConstructElement.ConstructGame(GameContext context)
        {
            buyManager = context.GetService<ProductBuyer>();
            moneyStorage = context.GetService<MoneyStorage>();
            resourceStorage = context.GetService<ResourceStorage>();
        }
    }
}