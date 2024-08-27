using Game.GameEngine.Mechanics;
using Game.Gameplay.Hero;
using GameSystem;

namespace Game.Meta
{
    public sealed class SpeedUpgrade : Upgrade, IGameInitElement
    {
        private readonly SpeedUpgradeConfig config;

        [GameInject]
        private IHeroService heroService;

        private IComponent_SetMoveSpeed heroComponent;

        public SpeedUpgrade(SpeedUpgradeConfig config) : base(config)
        {
            this.config = config;
        }

        public override string CurrentStats
        {
            get { return config.speedTable.GetSpeed(Level).ToString("F2"); }
        }

        public override string NextImprovement
        {
            get { return config.speedTable.SpeedStep.ToString("F2"); }
        }

        protected override void LevelUp(int level)
        {
            SetSpeed(level);
        }

        private void SetSpeed(int level)
        {
            var speed = config.speedTable.GetSpeed(level);
            heroComponent.SetSpeed(speed);
        }

        void IGameInitElement.InitGame()
        {
            heroComponent = heroService.GetHero().Get<IComponent_SetMoveSpeed>();
            SetSpeed(Level);
        }
    }
}