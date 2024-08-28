using System;
using Game.GameEngine.Mechanics;
using Game.Gameplay.Hero;
using Game.Gameplay.Player;

namespace Game.Tutorial
{
    public sealed class ConvertResourceInspector
    {
        private IHeroService heroService;

        private Action callback;

        public void Construct(IHeroService heroService)
        {
            this.heroService = heroService;
        }

        public void Inspect(Action callback)
        {
            this.callback = callback;
            heroService
                .GetHero()
                .Get<ConveyorVisitInputZoneObserver>()
                .OnResourcesLoaded += OnResourcesLoaded;
        }

        private void OnResourcesLoaded()
        {
            CompleteQuest();
        }

        private void CompleteQuest()
        {
            heroService
                .GetHero()
                .Get<ConveyorVisitInputZoneObserver>()
                .OnResourcesLoaded -= OnResourcesLoaded;
            callback?.Invoke();
        }
    }
}