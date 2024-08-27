using Game.GameEngine.InventorySystem;
using GameSystem;

namespace Game.Meta
{
    public abstract class InventoryObserver : 
        IGameReadyElement,
        IGameFinishElement
    {
        protected StackableInventory inventory;

        public void SetInventory(StackableInventory inventory)
        {
            this.inventory = inventory;
        }

        public virtual void ReadyGame()
        {
            inventory.OnItemAdded += OnItemAdded;
            inventory.OnItemRemoved += OnItemRemoved;
        }

        public virtual void FinishGame()
        {
            inventory.OnItemAdded -= OnItemAdded;
            inventory.OnItemRemoved -= OnItemRemoved;
        }

        protected abstract void OnItemAdded(InventoryItem item);

        protected abstract void OnItemRemoved(InventoryItem item);
    }
}