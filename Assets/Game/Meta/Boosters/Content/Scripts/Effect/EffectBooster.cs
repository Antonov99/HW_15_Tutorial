using Entities;
using Game.GameEngine.Mechanics;
using Game.Gameplay.Hero;
using GameSystem;

namespace Game.Meta
{
    public sealed class EffectBooster : Booster
    {
        private readonly EffectBoosterConfig config;

        [GameInject]
        private IHeroService heroService;

        public EffectBooster(EffectBoosterConfig config) : base(config)
        {
            this.config = config;
        }

        protected override void OnStart()
        {
            var heroComponent = heroService.GetHero().Get<IComponent_Effector>();
            heroComponent.Apply(config.effect);
        }

        protected override void OnStop()
        {
            var heroComponent = heroService.GetHero().Get<IComponent_Effector>();
            heroComponent.Discard(config.effect);
        }
    }
}