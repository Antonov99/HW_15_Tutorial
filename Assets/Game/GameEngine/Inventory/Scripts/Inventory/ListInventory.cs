using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace Game.GameEngine.InventorySystem
{
    
    
    public class ListInventory
    {
        public event Action<InventoryItem> OnItemAdded;
        public event Action<InventoryItem> OnItemRemoved;

        [ReadOnly]
        [ShowInInspector]
        private readonly List<InventoryItem> items;

        public ListInventory()
        {
            items = new List<InventoryItem>();
        }

        public void SetupItems(InventoryItem[] item)
        {            
            items.Clear();
            items.AddRange(item);
        }

        public void AddItem(InventoryItem item)
        {
            items.Add(item);
            OnItemAdded?.Invoke(item);
        }

        public bool RemoveItem(InventoryItem item)
        {
            if (items.Remove(item))
            {
                OnItemRemoved?.Invoke(item);
                return true;
            }
            
            return false;
        }

        public bool IsItemExists(InventoryItem item)
        {
            return items.Contains(item);
        }

        public bool IsEmpty()
        {
            return items.Count <= 0;
        }

        public InventoryItem[] GetAllItems()
        {
            return items.ToArray();
        }

        public List<InventoryItem> GetAllItemsUnsafe()
        {
            return items;
        }

        public bool FindItemFirst(InventoryItemFlags flags, out InventoryItem item)
        {
            for (int i = 0, count = items.Count; i < count; i++)
            {
                item = items[i];
                if (item.FlagsExists(flags))
                {
                    return true;
                }
            }

            item = default;
            return false;
        }

        public bool FindItemFirst(string name, out InventoryItem item)
        {
            for (int i = 0, count = items.Count; i < count; i++)
            {
                item = items[i];
                if (item.Name == name)
                {
                    return true;
                }
            }

            item = default;
            return false;
        }

        public bool FindItemLast(string name, out InventoryItem item)
        {
            for (var i = items.Count - 1; i >= 0; i--)
            {
                item = items[i];
                if (item.Name == name)
                {
                    return true;
                }
            }
            
            item = default;
            return false;
        }

        public bool FindItemFirst(Func<InventoryItem, bool> predicate, out InventoryItem item)
        {
            for (int i = 0, count = items.Count; i < count; i++)
            {
                item = items[i];
                if (predicate.Invoke(item))
                {
                    return true;
                }
            }

            item = default;
            return false;
        }

        public InventoryItem[] FindItems(InventoryItemFlags flags)
        {
            var result = new List<InventoryItem>();
            for (int i = 0, count = items.Count; i < count; i++)
            {
                var item = items[i];
                if (item.FlagsExists(flags))
                {
                    result.Add(item);
                }
            }

            return result.ToArray();
        }

        public InventoryItem[] FindItems(string name)
        {
            var result = new List<InventoryItem>();
            for (int i = 0, count = items.Count; i < count; i++)
            {
                var item = items[i];
                if (item.Name == name)
                {
                    result.Add(item);
                }
            }

            return result.ToArray();
        }

        public int CountItems(InventoryItemFlags flags)
        {
            var result = 0;
            for (int i = 0, count = items.Count; i < count; i++)
            {
                var item = items[i];
                if (item.FlagsExists(flags))
                {
                    result++;
                }
            }

            return result;
        }

        public int CountItems(string itemName)
        {
            var result = 0;
            for (int i = 0, count = items.Count; i < count; i++)
            {
                var item = items[i];
                if (item.Name == itemName)
                {
                    result++;
                }
            }

            return result;
        }
    }
}