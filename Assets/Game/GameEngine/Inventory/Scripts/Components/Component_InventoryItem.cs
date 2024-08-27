using System;
using UnityEngine;

namespace Game.GameEngine.InventorySystem
{
    [Serializable]
    public sealed class Component_InventoryItem : IComponent_InventoryItem
    {
        public InventoryItem Item
        {
            get { return config.Prototype; }
        }

        [SerializeField]
        private InventoryItemConfig config;
    }
}