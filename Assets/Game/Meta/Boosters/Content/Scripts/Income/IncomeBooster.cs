using Game.Gameplay.Player;
using GameSystem;
using Game.GameEngine;
using Game.Gameplay;

namespace Game.Meta
{
    public sealed class IncomeBooster : Booster
    {
        [GameInject]
        private VendorInteractor vendorInteractor;

        private readonly IncomeBoosterConfig config;

        public IncomeBooster(IncomeBoosterConfig config) : base(config)
        {
            this.config = config;
        }

        protected override void OnStart()
        {
            vendorInteractor.IncomeMultiplier *= config.incomeCoefficient;
        }

        protected override void OnStop()
        {
            vendorInteractor.IncomeMultiplier /= config.incomeCoefficient; 
        }
    }
}