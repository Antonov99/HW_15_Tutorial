using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine.GameResources
{
    [AddComponentMenu("GameEngine/GameResources/Resource Source")]
    public class UResourceSource : MonoBehaviour, IResourceSource
    {
        public event Action<ResourceType, int> OnValueChanged
        {
            add { source.OnValueChanged += value; }
            remove { source.OnValueChanged -= value; }
        }

        public event Action OnSetuped
        {
            add { source.OnSetuped += value; }
            remove { source.OnSetuped -= value; }
        }

        public event Action OnCleared
        {
            add { source.OnCleared += value; }
            remove { source.OnCleared -= value; }
        }

        public int Count
        {
            get { return source.Count; }
        }

        [SerializeField]
        private ResourceSource source = new();

        public int this[ResourceType type]
        {
            get { return source[type]; }
            set { source[type] = value; }
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
        public void Setup(ResourceData[] resources)
        {
            source.Setup(resources);
        }

        [GUIColor(0, 1, 0)]
        [Button]
        public void Set(ResourceType type, int amount)
        {
            source[type] = amount;
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