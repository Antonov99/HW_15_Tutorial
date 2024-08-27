using Game.GameEngine.Mechanics;
using Game.Gameplay.Hero;
using GameSystem;

namespace Game.Meta
{
    public sealed class DamageUpgrade : Upgrade, IGameInitElement
    {
        private readonly DamageUpgradeConfig config;

        [GameInject]
        private IHeroService heroService;

        private IComponent_SetMeleeDamage heroComponent;
        
        public DamageUpgrade(DamageUpgradeConfig config) : base(config)
        {
            this.config = config;
        }

        public override string CurrentStats
        {
            get { return config.damageTable.GetDamage(Level).ToString(); }
        }

        public override string NextImprovement
        {
            get { return config.damageTable.DamageStep.ToString(); }
        }
        
        protected override void LevelUp(int level)
        {
            SetDamage(level);
        }

        private void SetDamage(int level)
        {
            var currentDamage = config.damageTable.GetDamage(level);
            heroComponent.SetDamage(currentDamage);
        }

        void IGameInitElement.InitGame()
        {
            heroComponent = heroService.GetHero().Get<IComponent_SetMeleeDamage>();
            SetDamage(Level);
        }
    }
}