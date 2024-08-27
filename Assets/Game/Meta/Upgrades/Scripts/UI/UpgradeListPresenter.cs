using System.Collections.Generic;
using Game.Gameplay.Player;
using GameSystem;
using Windows;
using UnityEngine;

namespace Game.Meta
{
    [AddComponentMenu(UpgradeExtensions.MENU_PATH + "Upgrade List Presenter")]
    public sealed class UpgradeListPresenter : MonoWindow, IGameConstructElement
    {
        [SerializeField]
        private UpgradeView viewPrefab;

        [SerializeField]
        private Transform viewsContainer;
        
        private UpgradesManager upgradesManager;

        private MoneyStorage moneyStorage;

        private readonly List<UpgradePresenter> presenters;

        private readonly List<UpgradeView> views;

        public UpgradeListPresenter()
        {
            presenters = new List<UpgradePresenter>();
            views = new List<UpgradeView>();
        }

        protected override void OnShow(object args)
        {
            CreateUpgrades();
            ShowUpgrades();
        }

        protected override void OnHide()
        {
            DestroyUpgrades();
        }

        private void CreateUpgrades()
        {
            var upgrades = upgradesManager.GetAllUpgrades();
            for (int i = 0, count = upgrades.Length; i < count; i++)
            {
                var view = Instantiate(viewPrefab, viewsContainer);
                views.Add(view);

                var model = upgrades[i];
                var presenter = new UpgradePresenter(model, view);
                presenter.Construct(upgradesManager, moneyStorage);
                presenters.Add(presenter);
            }
        }

        private void ShowUpgrades()
        {
            for (int i = 0, count = presenters.Count; i < count; i++)
            {
                var presenter = presenters[i];
                presenter.Start();
            }
        }

        private void DestroyUpgrades()
        {
            var count = presenters.Count;
            for (var i = 0; i < count; i++)
            {
                var presenter = presenters[i];
                presenter.Stop();

                var view = views[i];
                Destroy(view.gameObject); //Можно кэшировать
            }

            presenters.Clear();
            views.Clear();
        }

        void IGameConstructElement.ConstructGame(GameContext context)
        {
            upgradesManager = context.GetService<UpgradesManager>();
            moneyStorage = context.GetService<MoneyStorage>();
        }
    }
}