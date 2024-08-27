using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine.GameResources
{
    [AddComponentMenu("GameEngine/GameResources/Component «Resource Source»")]
    public sealed class UComponent_ResourceSource : MonoBehaviour, IComponent_ResourceSource
    {
        public event Action<ResourceType, int> OnResourcesChanged
        {
            add { table.OnValueChanged += value; }
            remove { table.OnValueChanged -= value; }
        }

        [SerializeField]
        private UResourceSource table;

        [Title("Methods")]
        [GUIColor(0, 1, 0)]
        [Button]
        public void PutResources(ResourceType type, int amount)
        {
            table.Plus(type, amount);
        }
        
        [GUIColor(0, 1, 0)]
        [Button]
        public void ExtractResources(ResourceType type, int amount)
        {
            table.Minus(type, amount);
        }

        public int GetSum()
        {
            return table.GetSum();
        }

        public void Clear()
        {
            table.Clear();
        }

        [GUIColor(0, 1, 0)]
        [Button]
        public void SetupResources(ResourceData[] resources)
        {
            table.Setup(resources);
        }

        public int GetResources(ResourceType type)
        {
            return table[type];
        }

        public ResourceData[] GetAllResources()
        {
            return table.GetAll();
        }
    }
}