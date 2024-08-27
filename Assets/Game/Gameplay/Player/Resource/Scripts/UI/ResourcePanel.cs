using Game.GameEngine.GameResources;
using Game.UI;
using UnityEngine;

namespace Game.Gameplay.Player
{
    public sealed class ResourcePanel : NumberItemDictionary<ResourceType>
    {
        [Space]
        [SerializeField]
        private ResourceInfoCatalog resourceCatalog;

        private void Awake()
        {
            CreateItems();
        }

        private void CreateItems()
        {
            var resourceConfigs = resourceCatalog.GetAllResources();
            for (int i = 0, count = resourceConfigs.Length; i < count; i++)
            {
                var config = resourceConfigs[i];
                AddItem(config.type, 0);
            }
        }

        protected override Sprite FindIcon(ResourceType key)
        {
            var resourceInfo = resourceCatalog.FindResource(key);
            return resourceInfo.icon;
        }
    }
}