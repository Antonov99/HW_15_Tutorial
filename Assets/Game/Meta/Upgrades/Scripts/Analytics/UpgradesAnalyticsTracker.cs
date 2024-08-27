using System;
using System.Collections.Generic;
using GameSystem;
using UnityEngine;

namespace Game.Meta
{
    public sealed class UpgradesAnalyticsTracker :
        IGameReadyElement,
        IGameFinishElement
    {
        private UpgradesManager upgradesManager;

        [GameInject]
        public void Construct(UpgradesManager upgradesManager)
        {
            this.upgradesManager = upgradesManager;
        }

        void IGameReadyElement.ReadyGame()
        {
            upgradesManager.OnLevelUp += OnLevelUpUpgrade;
        }

        void IGameFinishElement.FinishGame()
        {
            upgradesManager.OnLevelUp -= OnLevelUpUpgrade;
        }

        private void OnLevelUpUpgrade(Upgrade upgrade)
        {
            UpgradesAnalytics.LogLevelUpUpgrade(upgrade);
        }
    }
}