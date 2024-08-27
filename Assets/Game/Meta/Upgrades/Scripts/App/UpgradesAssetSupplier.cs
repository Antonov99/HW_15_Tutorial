using Game.App;
using UnityEngine;

namespace Game.Meta
{
    public sealed class UpgradesAssetSupplier : IConfigLoader
    {
        private const string UPGRADE_CATALOG = "UpgradeCatalog";

        private UpgradeCatalog catalog;

        public UpgradeConfig GetUpgrade(string id)
        {
            return catalog.FindUpgrade(id);
        }

        public UpgradeConfig[] GetAllUpgrades()
        {
            return catalog.GetAllUpgrades();
        }

        void IConfigLoader.LoadConfigs()
        {
            catalog = Resources.Load<UpgradeCatalog>(UPGRADE_CATALOG);
        }
    }
}