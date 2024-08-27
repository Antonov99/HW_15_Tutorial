using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GameEngine.GameResources
{
    [Serializable]
    public sealed class ResourceSource : IResourceSource
    {
        public event Action<ResourceType, int> OnValueChanged;

        public event Action OnSetuped;

        public event Action OnCleared;

        public int Count
        {
            get { return GetSum(); }
        }

        [SerializeField]
        private List<ResourceData> resources = new();

        private List<ResourceData> buffer = new();

        public int this[ResourceType type]
        {
            get { return Get(type); }
            set { Set(type, value); }
        }

        public void Setup(ResourceData[] resources)
        {
            this.resources.Clear();

            for (int i = 0, count = resources.Length; i < count; i++)
            {
                var resource = resources[i];
                this.resources.Add(resource);
            }

            OnSetuped?.Invoke();
        }

        public void GetAllNonAlloc(Dictionary<ResourceType, int> result)
        {
            result.Clear();
            for (int i = 0, count = resources.Count; i < count; i++)
            {
                var data = resources[i];
                var amount = data.amount;
                if (amount > 0)
                {
                    result[data.type] = amount;
                }
            }
        }

        public void GetAllNonAlloc(List<ResourceData> result)
        {
            result.Clear();
            for (int i = 0, count = resources.Count; i < count; i++)
            {
                var data = resources[i];
                if (data.amount > 0)
                {
                    result.Add(data);
                }
            }
        }

        public ResourceData[] GetAll()
        {
            buffer.Clear();
            
            for (int i = 0, count = resources.Count; i < count; i++)
            {
                var data = resources[i];
                if (data.amount > 0)
                {
                    buffer.Add(data);
                }
            }

            return buffer.ToArray();
        }

        public bool Exists(ResourceType type, int requiredCount)
        {
            var currentAmount = Get(type);
            return currentAmount >= requiredCount;
        }

        public void Plus(ResourceType type, int range)
        {
            if (range <= 0)
            {
                return;
            }

            var previousCount = Get(type);
            var newCount = previousCount + range;
            Set(type, newCount);
        }

        public void Minus(ResourceType type, int range)
        {
            if (range <= 0)
            {
                return;
            }

            var previousCount = Get(type);
            var newCount = previousCount - range;
            newCount = Math.Max(newCount, 0);
            Set(type, newCount);
        }

        public void Clear()
        {
            resources.Clear();
            OnCleared?.Invoke();
        }

        private int Get(ResourceType type)
        {
            for (int i = 0, count = resources.Count; i < count; i++)
            {
                var resource = resources[i];
                if (resource.type == type)
                {
                    return resource.amount;
                }
            }

            return 0;
        }

        private void Set(ResourceType type, int newAmount)
        {
            if (newAmount < 0)
            {
                throw new Exception($"Set negative amount {newAmount} of resource type {type}!");
            }

            var result = new ResourceData(type, newAmount);

            for (int i = 0, count = resources.Count; i < count; i++)
            {
                var resource = resources[i];
                if (resource.type != type)
                {
                    continue;
                }

                resources[i] = result;
                OnValueChanged?.Invoke(type, newAmount);
                return;
            }

            resources.Add(result);
            OnValueChanged?.Invoke(type, newAmount);
        }

        public int GetSum()
        {
            var result = 0;
            for (int i = 0, count = resources.Count; i < count; i++)
            {
                var resource = resources[i];
                result += resource.amount;
            }

            return result;
        }
    }
}