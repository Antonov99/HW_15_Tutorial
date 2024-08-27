using Game.GameEngine;
using Game.GameEngine.InventorySystem;

namespace Game.Meta
{
    public sealed class InventoryItemViewPresenter
    {
        private readonly InventoryItemView view;

        private readonly InventoryItem item;

        private IComponent_Stackable stackableComponent;

        private PopupManager popupManager;

        private InventoryItemConsumer consumeManager;

        public InventoryItemViewPresenter(InventoryItemView view, InventoryItem item)
        {
            this.view = view;
            this.item = item;
        }

        public void Construct(PopupManager popupManager, InventoryItemConsumer consumeManager)
        {
            this.popupManager = popupManager;
            this.consumeManager = consumeManager;
        }

        public void Start()
        {
            var metadata = item.Metadata;
            view.SetTitle(metadata.title);
            view.SetIcon(metadata.icon);

            var flagsExists = item.FlagsExists(InventoryItemFlags.STACKABLE);
            view.Stack.SetVisible(flagsExists);

            if (flagsExists)
            {
                stackableComponent = item.GetComponent<IComponent_Stackable>();
                stackableComponent.OnValueChanged += OnAmountChanged;
                
                view.Stack.SetAmount(stackableComponent.Value, stackableComponent.Size);
            }

            view.AddClickListener(OnItemClicked);
        }

        public void Stop()
        {
            if (item.FlagsExists(InventoryItemFlags.STACKABLE))
            {
                stackableComponent.OnValueChanged -= OnAmountChanged;
            }

            view.RemoveClickListener(OnItemClicked);
        }

        private void OnAmountChanged(int newCount)
        {
            view.Stack.SetAmount(newCount, stackableComponent.Size);
        }

        private void OnItemClicked()
        {
            var presenter = new InventoryItemPresentationModel(item, consumeManager);
            popupManager.ShowPopup(PopupName.INVENTORY_ITEM, presenter);
        }
    }
}