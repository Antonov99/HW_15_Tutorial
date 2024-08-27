using System.Collections.Generic;
using Game.GameEngine;
using Game.GameEngine.InventorySystem;
using GameSystem;
using Windows;
using UnityEngine;

namespace Game.Meta
{
    public sealed class InventoryItemListPresenter : MonoWindow, IGameConstructElement
    {
        [SerializeField]
        private InventoryItemView prefab;

        [SerializeField]
        private Transform container;

        private InventoryService inventoryService;

        private PopupManager popupManager;

        private InventoryItemConsumer consumeManager;

        private readonly Dictionary<InventoryItem, ViewHolder> items = new();

        protected override void OnShow(object args)
        {
            var playerInventory = inventoryService.GetInventory();
            playerInventory.OnItemAdded += AddItem;
            playerInventory.OnItemRemoved += RemoveItem;

            var inventoryItems = playerInventory.GetAllItems();
            for (int i = 0, count = inventoryItems.Length; i < count; i++)
            {
                var inventoryItem = inventoryItems[i];
                AddItem(inventoryItem);
            }
        }

        protected override void OnHide()
        {
            var playerInventory = inventoryService.GetInventory();
            playerInventory.OnItemAdded -= AddItem;
            playerInventory.OnItemRemoved -= RemoveItem;

            var inventoryItems = playerInventory.GetAllItems();
            for (int i = 0, count = inventoryItems.Length; i < count; i++)
            {
                var inventoryItem = inventoryItems[i];
                RemoveItem(inventoryItem);
            }
        }

        private void AddItem(InventoryItem item)
        {
            if (items.ContainsKey(item))
            {
                return;
            }

            var view = Instantiate(prefab, container);
            var presenter = new InventoryItemViewPresenter(view, item);
            presenter.Construct(popupManager, consumeManager);

            var viewHolder = new ViewHolder(view, presenter);
            items.Add(item, viewHolder);

            presenter.Start();
        }

        private void RemoveItem(InventoryItem item)
        {
            if (!items.ContainsKey(item))
            {
                return;
            }

            var viewHolder = items[item];
            viewHolder.presenter.Stop();
            Destroy(viewHolder.view.gameObject);
            items.Remove(item);
        }

        void IGameConstructElement.ConstructGame(GameContext context)
        {
            inventoryService = context.GetService<InventoryService>();
            popupManager = context.GetService<PopupManager>();
            consumeManager = context.GetService<InventoryItemConsumer>();
        }

        private sealed class ViewHolder
        {
            public readonly InventoryItemView view;
            public readonly InventoryItemViewPresenter presenter;

            public ViewHolder(InventoryItemView view, InventoryItemViewPresenter presenter)
            {
                this.view = view;
                this.presenter = presenter;
            }
        }
    }
}