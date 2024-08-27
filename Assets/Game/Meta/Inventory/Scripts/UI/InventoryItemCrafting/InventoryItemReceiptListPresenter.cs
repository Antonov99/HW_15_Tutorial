using System;
using System.Collections.Generic;
using Game.GameEngine.InventorySystem;
using GameSystem;
using Windows;
using UnityEngine;

namespace Game.Meta
{
    public sealed class InventoryItemReceiptListPresenter : MonoWindow, IGameConstructElement
    {
        [SerializeField]
        private InventoryItemReceiptCatalog receiptCatalog;

        [SerializeField]
        private InventoryItemReceiptView viewPrefab;

        [SerializeField]
        private Transform container;

        private InventoryItemCrafter craftManager;

        private StackableInventory inventory;

        private readonly List<InventoryItemReceiptPresenter> presenters = new();

        private bool receiptsCreated;

        protected override void OnShow(object args)
        {
            if (!receiptsCreated)
            {
                CreateReceipts();
                receiptsCreated = true;
            }
            
            for (int i = 0, count = presenters.Count; i < count; i++)
            {
                var presenter = presenters[i];
                presenter.Start();
            }
        }

        protected override void OnHide()
        {
            for (int i = 0, count = presenters.Count; i < count; i++)
            {
                var presenter = presenters[i];
                presenter.Stop();
            }
        }

        void IGameConstructElement.ConstructGame(GameContext context)
        {
            inventory = context.GetService<InventoryService>().GetInventory();
            craftManager = context.GetService<InventoryItemCrafter>();
        }

        private void CreateReceipts()
        {
            var receipts = receiptCatalog.GetAllReceipts();
            for (int i = 0, count = receipts.Length; i < count; i++)
            {
                var receipt = receipts[i];
                CreateReceipt(receipt);
            }
        }

        private void CreateReceipt(InventoryItemReceipt receipt)
        {
            var view = Instantiate(viewPrefab, container);
            var presenter = new InventoryItemReceiptPresenter(view, receipt);
            presenter.Construct(craftManager, inventory);
            presenters.Add(presenter);
        }
    }
}