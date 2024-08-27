using Game.GameEngine.InventorySystem;
using UnityEngine;

namespace Game.Meta
{
    public sealed class InventoryItemPresentationModel : IInventoryItemPresentationModel
    {
        public string Title
        {
            get { return item.Metadata.title; }
        }

        public string Description
        {
            get { return item.Metadata.decription; }
        }

        public Sprite Icon
        {
            get { return item.Metadata.icon; }
        }

        private readonly InventoryItem item;

        private readonly InventoryItemConsumer consumeManager;

        public InventoryItemPresentationModel(InventoryItem item, InventoryItemConsumer consumeManager)
        {
            this.item = item;
            this.consumeManager = consumeManager;
        }
        
        public bool IsStackableItem()
        {
            return item.FlagsExists(InventoryItemFlags.STACKABLE);
        }

        public void GetStackInfo(out int current, out int size)
        {
            var component = item.GetComponent<IComponent_Stackable>();
            current = component.Value;
            size = component.Size;
        }

        public bool IsConsumableItem()
        {
            return item.FlagsExists(InventoryItemFlags.CONSUMABLE);
        }

        public bool CanConsumeItem()
        {
            return consumeManager.CanConsumeItem(item);
        }

        public void OnConsumeClicked()
        {
            if (consumeManager.CanConsumeItem(item))
            {
                consumeManager.ConsumeItem(item);
            }
        }
    }
}