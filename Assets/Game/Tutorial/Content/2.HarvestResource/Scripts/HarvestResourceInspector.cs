using System;
using Game.GameEngine.Mechanics;
using Game.Gameplay.Hero;

namespace Game.Tutorial
{
    public sealed class HarvestResourceInspector
    {
        private HarvestResourceConfig config;

        private IHeroService heroService;

        private Action callback;

        public void Construct(IHeroService heroService, HarvestResourceConfig config)
        {
            this.heroService = heroService;
            this.config = config;
        }

        public void Inspect(Action callback)
        {
            this.callback = callback;
            heroService
                .GetHero()
                .Get<IComponent_HarvestResource>()
                .OnHarvestStopped += OnHarvestFinished;
        }

        private void OnHarvestFinished(HarvestResourceOperation operation)
        {
            if (!operation.isCompleted)
            {
                return;
            }
            
            if (operation.resourceType == config.targetResourceType)
            {
                CompleteQuest();
            }
        }

        private void CompleteQuest()
        {
            heroService
                .GetHero()
                .Get<IComponent_HarvestResource>()
                .OnHarvestStopped -= OnHarvestFinished;
            callback?.Invoke();
        }
    }
}