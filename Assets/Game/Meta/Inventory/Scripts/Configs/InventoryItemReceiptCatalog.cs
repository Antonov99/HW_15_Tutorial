using System;
using Game.GameEngine.InventorySystem;
using UnityEngine;

namespace Game.Meta
{
    [CreateAssetMenu(
        fileName = "InventoryItemReceiptCatalog",
        menuName = "Meta/Inventory/New InventoryItemReceiptCatalog"
    )]
    public sealed class InventoryItemReceiptCatalog : ScriptableObject
    {
        [SerializeField]
        private InventoryItemReceipt[] receipts;

        public InventoryItemReceipt[] GetAllReceipts()
        {
            return receipts;
        }

        public InventoryItemReceipt FindReceipt(string name)
        {
            for (int i = 0, count = receipts.Length; i < count; i++)
            {
                var receipt = receipts[i];
                if (receipt.resultInfo.ItemName == name)
                {
                    return receipt;
                }
            }
            
            throw new Exception($"Receipt with name {name} is not found!");
        }
    }
}