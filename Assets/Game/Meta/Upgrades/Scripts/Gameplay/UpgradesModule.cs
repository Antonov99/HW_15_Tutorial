using System.Collections.Generic;
using Game.Gameplay.Player;
using GameSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Meta
{
    public sealed class UpgradesModule : GameModule
    {
        [SerializeField]
        private UpgradeCatalog catalog;

        [ShowInInspector]
        private UpgradesManager upgradesManager = new();
        
        private Upgrade[] upgrades;

        [Space, ReadOnly, ShowInInspector]
        private UpgradesAnalyticsTracker analyticsTracker = new();

        public override IEnumerable<object> GetServices()
        {
            yield return upgradesManager;
        }

        public override IEnumerable<IGameElement> GetElements()
        {
            for (int i = 0, count = upgrades.Length; i < count; i++)
            {
                if (upgrades[i] is IGameElement element)
                {
                    yield return element;
                }
            }

            yield return analyticsTracker;
        }

        public override void ConstructGame(GameContext context)
        {
            ConstructUpgrades(context);
            ConstructManager(context);
            ConstructAnalytics();
        }

        private void ConstructUpgrades(GameContext context)
        {
            GameInjector.InjectAll(context, upgrades);
        }

        private void ConstructManager(GameContext context)
        {
            var moneyStorage = context.GetService<MoneyStorage>();
            upgradesManager.Construct(moneyStorage);
            upgradesManager.Setup(upgrades);
        }

        private void ConstructAnalytics()
        {
            analyticsTracker.Construct(upgradesManager);
        }

        private void Awake()
        {
            CreateUpgrades();
        }

        private void CreateUpgrades()
        {
            var configs = catalog.GetAllUpgrades();
            var count = configs.Length;
            upgrades = new Upgrade[count];

            for (var i = 0; i < count; i++)
            {
                var config = configs[i];
                upgrades[i] = config.InstantiateUpgrade();
            }
        }
    }
}