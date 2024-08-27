using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine.GameResources
{
    [AddComponentMenu("GameEngine/GameResources/Component «Resource Source Limited»")]
    public sealed class UComponent_ResourceSourceLimited : MonoBehaviour, IComponent_ResourceSourceLimited
    {
        public event Action<ResourceType, int> OnResourcesChanged
        {
            add { stack.OnValueChanged += value; }
            remove { stack.OnValueChanged -= value; }
        }

        public event Action<int> OnLimitChanged
        {
            add { stack.OnLimitChanged += value; }
            remove { stack.OnLimitChanged -= value; }
        }

        public int AvailableSlots
        {
            get { return stack.AvailableCount; }
        }
        
        public int Limit
        {
            get { return stack.Limit; }
        }

        public bool IsLimit
        {
            get { return stack.IsLimit; }
        }

        [SerializeField]
        private UResourceSourceLimited stack;

        [Title("Methods")]
        [GUIColor(0, 1, 0)]
        [Button]
        public void PutResources(ResourceType type, int amount)
        {
            stack.Plus(type, amount);
        }

        [GUIColor(0, 1, 0)]
        [Button]
        public void SetupResources(ResourceData[] resources)
        {
            stack.Setup(resources);
        }
        
        [GUIColor(0, 1, 0)]
        [Button]
        public void ExtractResources(ResourceType type, int amount)
        {
            stack.Minus(type, amount);
        }

        public int GetSum()
        {
            return stack.GetSum();
        }

        public void Clear()
        {
            stack.Clear();
        }

        [GUIColor(0, 1, 0)]
        [Button]
        public void SetLimit(int limit)
        {
            stack.SetLimit(limit);
        }

        public int GetResources(ResourceType type)
        {
            return stack[type];
        }

        public ResourceData[] GetAllResources()
        {
            return stack.GetAll();
        }
    }
}