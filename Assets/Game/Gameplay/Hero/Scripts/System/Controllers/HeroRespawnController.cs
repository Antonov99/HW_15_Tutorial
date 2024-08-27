using Game.GameEngine.Mechanics;
using GameSystem;

namespace Game.Gameplay.Hero
{
    public sealed class HeroRespawnController : 
        IGameInitElement,
        IGameReadyElement,
        IGameFinishElement
    {
        private HeroService heroService;

        private RespawnInteractor respawnInteractor;

        private IComponent_OnDestroyed<DestroyArgs> heroComponent;

        [GameInject]
        public void Construct(HeroService heroService, RespawnInteractor respawnInteractor)
        {
            this.heroService = heroService;
            this.respawnInteractor = respawnInteractor;
        }

        void IGameInitElement.InitGame()
        {
            heroComponent = heroService.GetHero().Get<IComponent_OnDestroyed<DestroyArgs>>();
        }

        void IGameReadyElement.ReadyGame()
        {
            heroComponent.OnDestroyed += OnHeroDestroyed;
        }

        void IGameFinishElement.FinishGame()
        {
            heroComponent.OnDestroyed -= OnHeroDestroyed;
        }

        private void OnHeroDestroyed(DestroyArgs destroyArgs)
        {
            respawnInteractor.StartRespawn();
        }
    }
}