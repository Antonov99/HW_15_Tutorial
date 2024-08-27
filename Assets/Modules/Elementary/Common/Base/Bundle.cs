using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Elementary
{
    public class Bundle : IEnumerable
    {
        private static readonly object FLAG = new();

        [ReadOnly]
        [ShowInInspector]
        protected readonly Dictionary<string, object> items;

        public Bundle()
        {
            items = new Dictionary<string, object>();
        }

        public Bundle(IDictionary<string, object> items)
        {
            this.items = new Dictionary<string, object>(items);
        }

        public T Get<T>(string key)
        {
            return (T) items[key];
        }

        public bool TryGet<T>(string key, out T result)
        {
            if (items.TryGetValue(key, out var value))
            {
                result = (T) value;
                return true;
            }

            result = default;
            return false;
        }

        public bool Has(string key)
        {
            return items.ContainsKey(key);
        }

        public Bundle Add(string key)
        {
            items.Add(key, FLAG);
            return this;
        }

        public Bundle Add(string key, object element)
        {
            items.Add(key, element);
            return this;
        }

        public Bundle Remove(string key)
        {
            items.Remove(key);
            return this;
        }

        public IEnumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}