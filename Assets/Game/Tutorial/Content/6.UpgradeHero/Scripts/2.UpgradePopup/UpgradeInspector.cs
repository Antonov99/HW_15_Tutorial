using System;
using Game.Meta;

namespace Game.Tutorial
{
    public sealed class UpgradeInspector
    {
        private UpgradeHeroConfig config;
        
        private UpgradesManager upgradesManager;

        private Upgrade targetUpgrade;
        
        private Action callback;
        
        public void Construct(UpgradesManager upgradesManager, UpgradeHeroConfig targetConfig)
        {
            this.upgradesManager = upgradesManager;
            config = targetConfig;
        }
        
        public void Inspect(Action callback)
        {
            this.callback = callback;
            targetUpgrade = upgradesManager.GetUpgrade(config.upgradeConfig.id);
            targetUpgrade.OnLevelUp += OnLevelUp;
        }

        private void OnLevelUp(int nextLevel)
        {
            if (nextLevel < config.targetLevel)
            {
                return;
            }
            
            targetUpgrade.OnLevelUp -= OnLevelUp;
            targetUpgrade = null;
            callback?.Invoke();
        }
    }
}