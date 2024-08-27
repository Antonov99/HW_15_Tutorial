using Game.GameEngine.Mechanics;
using Game.Gameplay.Hero;
using GameSystem;

namespace Game.Meta
{
    public sealed class HitPointsUpgrade : Upgrade, IGameInitElement
    {
        private readonly HitPointsUpgradeConfig config;

        [GameInject]
        private IHeroService heroService;

        private IComponent_SetupHitPoints heroComponent;

        public HitPointsUpgrade(HitPointsUpgradeConfig config) : base(config)
        {
            this.config = config;
        }

        public override string CurrentStats
        {
            get { return config.hitPointsTable.GetHitPoints(Level).ToString(); }
        }

        public override string NextImprovement
        {
            get { return config.hitPointsTable.HitPointsStep.ToString(); }
        }

        protected override void LevelUp(int level)
        {
            SetHitPoints(level);
        }

        private void SetHitPoints(int level)
        {
            var hitPoints = config.hitPointsTable.GetHitPoints(level);
            heroComponent.Setup(hitPoints, hitPoints);
        }

        void IGameInitElement.InitGame()
        {
            heroComponent = heroService.GetHero().Get<IComponent_SetupHitPoints>();
            SetHitPoints(Level);
        }
    }
}