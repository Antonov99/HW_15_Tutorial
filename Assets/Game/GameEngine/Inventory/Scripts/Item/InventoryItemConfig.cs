using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Game.GameEngine.InventorySystem
{
    [CreateAssetMenu(
        fileName = "InventoryItemConfig",
        menuName = "GameEngine/Inventory/New InventoryItem"
    )]
    public sealed class InventoryItemConfig : SerializedScriptableObject
    {
        public string ItemName
        {
            get { return origin.Name; }
        }

        public InventoryItemMetadata Metadata
        {
            get { return origin.Metadata; }
        }

        public InventoryItemFlags Flags
        {
            get { return origin.Flags; }
        }

        public InventoryItem Prototype
        {
            get { return origin; }
        }

        [OdinSerialize]
        private InventoryItem origin = new();
    }
}