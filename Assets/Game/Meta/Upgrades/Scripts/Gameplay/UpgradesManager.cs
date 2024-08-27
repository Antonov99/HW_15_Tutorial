using System;
using System.Collections.Generic;
using System.Linq;
using Game.Gameplay.Player;
using Sirenix.OdinInspector;

namespace Game.Meta
{
    [Serializable]
    public sealed class UpgradesManager
    {
        public event Action<Upgrade> OnLevelUp;
        
        [ReadOnly, ShowInInspector]
        private Dictionary<string, Upgrade> upgrades = new();

        private MoneyStorage moneyStorage;

        public void Construct(MoneyStorage moneyStorage)
        {
            this.moneyStorage = moneyStorage;
        }

        public void Setup(Upgrade[] upgrades)
        {
            this.upgrades = new Dictionary<string, Upgrade>();
            for (int i = 0, count = upgrades.Length; i < count; i++)
            {
                var upgrade = upgrades[i];
                this.upgrades[upgrade.Id] = upgrade;
            }
        }

        public Upgrade GetUpgrade(string id)
        {
            return upgrades[id];
        }

        public Upgrade[] GetAllUpgrades()
        {
            return upgrades.Values.ToArray<Upgrade>();
        }

        public bool CanLevelUp(Upgrade upgrade)
        {
            if (upgrade.IsMaxLevel)
            {
                return false;
            }

            var price = upgrade.NextPrice;
            return moneyStorage.CanSpendMoney(price);
        }

        public void LevelUp(Upgrade upgrade)
        {
            if (!CanLevelUp(upgrade))
            {
                throw new Exception($"Can not level up {upgrade.Id}");
            }

            var price = upgrade.NextPrice;
            moneyStorage.SpendMoney(price);

            upgrade.LevelUp();
            OnLevelUp?.Invoke(upgrade);
        }

        [Title("Methods")]
        [Button]
        public bool CanLevelUp(string id)
        {
            var upgrade = upgrades[id];
            return CanLevelUp(upgrade);
        }

        [Button]
        public void LevelUp(string id)
        {
            var upgrade = upgrades[id];
            LevelUp(upgrade);
        }
    }
}