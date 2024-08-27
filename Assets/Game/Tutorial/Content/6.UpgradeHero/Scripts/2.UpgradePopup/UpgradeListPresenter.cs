using System;
using System.Collections.Generic;
using System.Linq;
using Game.Gameplay.Player;
using Game.Meta;
using GameSystem;
using UnityEngine;

namespace Game.Tutorial
{
    public sealed class UpgradeListPresenter : MonoBehaviour, IGameConstructElement
    {
        [SerializeField]
        private UpgradeHeroConfig config;

        [SerializeField]
        private UpgradeView targetView;

        [SerializeField]
        private UpgradeView[] otherViews;

        private UpgradesManager upgradesManager;

        private MoneyStorage moneyStorage;

        private readonly List<UpgradePresenter> presenters;

        public UpgradeListPresenter()
        {
            presenters = new List<UpgradePresenter>();
        }

        void IGameConstructElement.ConstructGame(GameContext context)
        {
            upgradesManager = context.GetService<UpgradesManager>();
            moneyStorage = context.GetService<MoneyStorage>();
        }

        public void Show()
        {
            InitUpgrades();
            ShowUpgrades();
        }

        public void Hide()
        {
            HideUpgrades();
            presenters.Clear();
        }

        private void InitUpgrades()
        {
            var targetId = config.upgradeConfig.id;
            var targetUprade = upgradesManager.GetUpgrade(targetId);
            CreatePresenter(targetUprade, targetView);

            var otherUpgrades = upgradesManager
                .GetAllUpgrades()
                .Where(it => it.Id != targetId)
                .ToArray();

            var otherCount = Math.Min(otherViews.Length, otherUpgrades.Length);
           
            for (var i = 0 ; i < otherCount; i++)
            {
                var upgrade = otherUpgrades[i];
                var view = otherViews[i];
                CreatePresenter(upgrade, view);
            }
        }

        private void CreatePresenter(Upgrade targetUprade, UpgradeView view)
        {
            var targetPresenter = new UpgradePresenter(targetUprade, view);
            targetPresenter.Construct(upgradesManager, moneyStorage);
            presenters.Add(targetPresenter);
        }

        private void ShowUpgrades()
        {
            for (int i = 0, count = presenters.Count; i < count; i++)
            {
                var presenter = presenters[i];
                presenter.Start();
            }
        }

        private void HideUpgrades()
        {
            for (int i = 0, count = presenters.Count; i < count; i++)
            {
                var presenter = presenters[i];
                presenter.Stop();
            }
        }
    }
}