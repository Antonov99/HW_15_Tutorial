using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine.GameResources
{
    [AddComponentMenu("GameEngine/GameResources/Resource Source «Limited»")]
    public sealed class UResourceSourceLimited : MonoBehaviour, IResourceSource
    {
        public event Action<int> OnLimitChanged;

        public event Action<ResourceType, int> OnValueChanged
        {
            add { source.OnValueChanged += value; }
            remove { source.OnValueChanged -= value; }
        }

        public event Action OnSetuped;

        public event Action OnCleared
        {
            add { source.OnCleared += value; }
            remove { source.OnCleared -= value; }
        }

        [PropertySpace]
        [PropertyOrder(-10)]
        [ReadOnly]
        [ShowInInspector]
        public int AvailableCount
        {
            get { return limit - Count; }
        }

        [PropertyOrder(-9)]
        [ReadOnly]
        [ShowInInspector]
        public int Count
        {
            get { return source.Count; }
        }

        public int Limit
        {
            get { return limit; }
        }

        [PropertyOrder(-8)]
        [ReadOnly]
        [ShowInInspector]
        public bool IsLimit
        {
            get { return Count >= limit; }
        }

        [Title("Fields")]
        [SerializeField]
        private int limit;

        [SerializeField]
        private ResourceSource source = new ResourceSource();

        public int this[ResourceType type]
        {
            get { return source[type]; }
        }

        public ResourceData[] GetAll()
        {
            return source.GetAll();
        }

        public void GetAllNonAlloc(Dictionary<ResourceType, int> result)
        {
            source.GetAllNonAlloc(result);
        }

        public void GetAllNonAlloc(List<ResourceData> result)
        {
            source.GetAllNonAlloc(result);
        }

        [Title("Methods")]
        [GUIColor(0, 1, 0)]
        [Button]
        public void Setup(ResourceData[] resources, int limit)
        {
            source.Setup(resources);
            this.limit = limit;
            OnSetuped?.Invoke();
        }

        [GUIColor(0, 1, 0)]
        [Button]
        public void Setup(ResourceData[] resources)
        {
            source.Setup(resources);
            OnSetuped?.Invoke();
        }

        [GUIColor(0, 1, 0)]
        [Button]
        public bool Exists(ResourceType type, int requiredCount)
        {
            return source.Exists(type, requiredCount);
        }

        [GUIColor(0, 1, 0)]
        [Button]
        public void Plus(ResourceType type, int range)
        {
            if (range <= 0)
            {
                return;
            }

            var resourceCount = Count;
            if (resourceCount >= limit)
            {
                return;
            }

            var newCount = resourceCount + range;
            if (newCount > limit)
            {
                range = limit - resourceCount;
            }

            source.Plus(type, range);
        }

        [GUIColor(0, 1, 0)]
        [Button]
        public void Minus(ResourceType type, int range)
        {
            source.Minus(type, range);
        }

        [GUIColor(0, 1, 0)]
        [Button]
        public void SetLimit(int limit)
        {
            this.limit = limit;
            OnLimitChanged?.Invoke(limit);
        }

        [GUIColor(0, 1, 0)]
        [Button]
        public void Clear()
        {
            source.Clear();
        }

        public int GetSum()
        {
            return source.GetSum();
        }
    }
}