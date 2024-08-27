using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
// ReSharper disable UnusedMethodReturnValue.Local

namespace Game.GameEngine.InventorySystem
{
    public class StackableInventory
    {
        public event Action<InventoryItem> OnItemAdded;
        public event Action<InventoryItem> OnItemRemoved;

        private readonly ListInventory list;

        public StackableInventory(ListInventory list)
        {
            this.list = list;
        }

        public StackableInventory()
        {
            list = new ListInventory();
        }

        public InventoryItem[] GetAllItems()
        {
            return list.GetAllItems();
        }

        public List<InventoryItem> GetAllItemsUnsafe()
        {
            return list.GetAllItemsUnsafe();
        }

        public bool FindItemFirst(string name, out InventoryItem item)
        {
            return list.FindItemFirst(name, out item);
        }

        public bool FindItemFirst(InventoryItemFlags tags, out InventoryItem item)
        {
            return list.FindItemFirst(tags, out item);
        }

        public bool FindItemLast(string name, out InventoryItem item)
        {
            return list.FindItemLast(name, out item);
        }

        public InventoryItem[] FindItems(string name)
        {
            return list.FindItems(name);
        }

        public InventoryItem[] FindItems(InventoryItemFlags tags)
        {
            return list.FindItems(tags);
        }

        public bool IsEmpty()
        {
            return list.IsEmpty();
        }

        public bool IsItemExists(InventoryItem item)
        {
            return list.IsItemExists(item);
        }
        
        public bool SetupItem(InventoryItem item)
        {
            if (item is null)
            {
                return false;
            }
        
            if (IsItemExists(item))
            {
                return false;
            }

            list.AddItem(item);
            return true;
        }
        
        /// <summary>
        ///     <para>Adds one item in a new slot</para>
        /// </summary>
        public bool AddItemAsInstance(InventoryItem item)
        {
            if (item is null)
            {
                return false;
            }
        
            if (IsItemExists(item))
            {
                return false;
            }

            list.AddItem(item);
            OnItemAdded?.Invoke(item);
            return true;
        }
        
        /// <summary>
        ///     <para>Removes one item</para>
        /// </summary>
        /// <param name="item">Target item in inventory.</param>
        public bool RemoveItem(InventoryItem item)
        {
            if (item is null)
            {
                return false;
            }

            if (item.FlagsExists(InventoryItemFlags.STACKABLE))
            {
                return RemoveAsStackable(item);
            }

            return RemoveAsInstance(item);
        }

        public bool RemoveItem(string itemName)
        {
            if (!list.FindItemLast(itemName, out var item))
            {
                return false;
            }

            if (item.FlagsExists(InventoryItemFlags.STACKABLE))
            {
                return RemoveAsStackable(item);
            }

            return RemoveAsInstance(item);
        }

        /// <summary>
        ///     <para>Removes several items by name</para>
        /// </summary>
        /// <param name="itemName">Target item name</param>
        /// <param name="count">Expected item count to remove</param>
        public void RemoveItems(string itemName, int count)
        {
            while (count > 0)
            {
                if (!list.FindItemLast(itemName, out var item))
                {
                    return;
                }

                if (item.FlagsExists(InventoryItemFlags.STACKABLE))
                {
                    DecrementValueInStack(item, ref count);
                }
                else
                {
                    RemoveAsInstance(item);
                }
            }
        }
        
        private bool RemoveAsStackable(InventoryItem item)
        {
            if (!list.IsItemExists(item))
            {
                return false;
            }

            var component = item.GetComponent<IComponent_Stackable>();
            component.Value--;
            if (component.Value > 0)
            {
                return true;
            }

            if (list.RemoveItem(item))
            {
                OnItemRemoved?.Invoke(item);
            }

            return true;
        }

        private bool RemoveAsInstance(InventoryItem item)
        {
            if (list.RemoveItem(item))
            {
                OnItemRemoved?.Invoke(item);
                return true;
            }

            return false;
        }

        public int CountAllItems()
        {
            var result = 0;

            var items = list.GetAllItemsUnsafe();
            for (int i = 0, count = items.Count; i < count; i++)
            {
                var item = items[i];
                result += CountItem(item);
            }

            return result;
        }

        public Dictionary<string, int> CountAllItemsInDictionary()
        {
            var result = new Dictionary<string, int>();
            var items = list.GetAllItemsUnsafe();
            for (int i = 0, count = items.Count; i < count; i++)
            {
                var item = items[i];
                var itemName = item.Name;
                result.TryGetValue(itemName, out var amount);
                
                if (item.FlagsExists(InventoryItemFlags.STACKABLE))
                {
                    amount += item.GetComponent<IComponent_Stackable>().Value;
                }
                else
                {
                    amount++;
                }
                
                result[itemName] = amount;
            }

            return result;
        }

        public int CountItems(string itemName)
        {
            var result = 0;

            var items = list.FindItems(itemName);
            for (int i = 0, count = items.Length; i < count; i++)
            {
                var item = items[i];
                result += CountItem(item);
            }

            return result;
        }

        private int CountItem(InventoryItem item)
        {
            if (item.FlagsExists(InventoryItemFlags.STACKABLE))
            {
                return item.GetComponent<IComponent_Stackable>().Value;
            }

            return 1;
        }
        
        /// <summary>
        ///     <para>Spawns items by prototype.</para>
        /// </summary>
        /// <param name="prototype">Origin item (will not be added to inventory)</param>
        /// <param name="count">Required items to add.</param>
        public void AddItemsByPrototype(InventoryItem prototype, int count)
        {
            if (prototype.FlagsExists(InventoryItemFlags.STACKABLE))
            {
                SpawnAsStackable(prototype, count);
            }
            else
            {
                SpawnAsSingle(prototype, count);
            }
        }

        private void SpawnAsStackable(InventoryItem prototype, int count)
        {
            var itemName = prototype.Name;
            var stackSize = prototype.GetComponent<IComponent_Stackable>().Size;

            while (count > 0)
            {
                if (list.FindItemFirst(IsAvailable, out var targetItem))
                {
                    IncrementValueInStack(targetItem, stackSize, ref count);
                }
                else
                {
                    targetItem = prototype.Clone();
                    IncrementValueInStack(targetItem, stackSize, ref count);
                    list.AddItem(targetItem);
                    OnItemAdded?.Invoke(targetItem);
                }
            }

            bool IsAvailable(InventoryItem it)
            {
                return it.Name == itemName && !it.GetComponent<IComponent_Stackable>().IsFull;
            }
        }

        private void SpawnAsSingle(InventoryItem prototype, int count)
        {
            for (var i = 0; i < count; i++)
            {
                SpawnAsSingle(prototype);
            }
        }

        private void SpawnAsSingle(InventoryItem prototype)
        {
            var item = prototype.Clone();
            list.AddItem(item);
            OnItemAdded?.Invoke(item);
        }

        private void IncrementValueInStack(InventoryItem item, int stackSize, ref int remainingCount)
        {
            var stackableComponent = item.GetComponent<IComponent_Stackable>();
            var previousCount = stackableComponent.Value;
            var newCount = previousCount + remainingCount;

            var overflow = newCount - stackSize;
            if (overflow > 0)
            {
                newCount = stackSize;
            }

            stackableComponent.Value = newCount;

            var diff = newCount - previousCount;
            remainingCount -= diff;
        }
        
        private void DecrementValueInStack(InventoryItem item, ref int remainingCount)
        {
            var stackableComponent = item.GetComponent<IComponent_Stackable>();
            var previousCount = stackableComponent.Value;
            var newCount = previousCount - remainingCount;

            if (newCount <= 0)
            {
                newCount = 0;
                stackableComponent.Value = 0;
                RemoveAsInstance(item);
            }
            else
            {
                stackableComponent.Value = newCount;
            }

            var diff = previousCount - newCount;
            remainingCount -= diff;
        }


#if UNITY_EDITOR
        [PropertySpace(8)]
        [PropertyOrder(-10)]
        [ReadOnly]
        [ShowInInspector]
        private List<InventoryItem> _items
        {
            get { return list.GetAllItemsUnsafe(); }
        }
#endif
    }
}