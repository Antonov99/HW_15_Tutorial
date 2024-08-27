using Game.App;
using Game.GameEngine.InventorySystem;
using UnityEngine;

namespace Game.Meta
{
    public sealed class InventoryItemsAssetSupplier : IConfigLoader
    {
        private InventoryItemCatalog catalog;

        public InventoryItemConfig GetItem(string name)
        {
            return catalog.FindItem(name);
        }

        public InventoryItemConfig[] GetAllItems()
        {
            return catalog.GetAllItems();
        }

        void IConfigLoader.LoadConfigs()
        {
            catalog = Resources.Load<InventoryItemCatalog>("InventoryItemCatalog");
        }
    }
}