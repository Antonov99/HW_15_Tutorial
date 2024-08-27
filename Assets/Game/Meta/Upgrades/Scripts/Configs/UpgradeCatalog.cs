using System;
using UnityEngine;

namespace Game.Meta
{
    [CreateAssetMenu(
        fileName = "UpgradeCatalog",
        menuName = UpgradeExtensions.MENU_PATH + "New UpgradeCatalog"
    )]
    public sealed class UpgradeCatalog : ScriptableObject
    {
        [SerializeField]
        private UpgradeConfig[] configs;
        
        public UpgradeConfig[] GetAllUpgrades()
        {
            return configs;
        }

        public UpgradeConfig FindUpgrade(string id)
        {
            var length = configs.Length;
            for (var i = 0; i < length; i++)
            {
                var config = configs[i];
                if (config.id == id)
                {
                    return config;
                }
            }

            throw new Exception($"Config with {id} is not found!");
        }
    }
}