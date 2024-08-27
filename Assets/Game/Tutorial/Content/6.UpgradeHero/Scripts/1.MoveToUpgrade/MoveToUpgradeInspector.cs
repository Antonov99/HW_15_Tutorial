using System;
using Game.GameEngine;
using Game.Gameplay.Player;

namespace Game.Tutorial
{
    public sealed class MoveToUpgradeInspector
    {
        private WorldPlaceVisitInteractor worldPlaceVisitor;

        private UpgradeHeroConfig config;

        private Action callback;

        public void Construct(WorldPlaceVisitInteractor worldPlaceVisitor, UpgradeHeroConfig config)
        {
            this.worldPlaceVisitor = worldPlaceVisitor;
            this.config = config;
        }

        public void Inspect(Action callback)
        {
            this.callback = callback;
            worldPlaceVisitor.OnVisitStarted += OnPlaceVisited;
        }
        
        private void OnPlaceVisited(WorldPlaceType placeType)
        {
            if (placeType == config.worldPlaceType)
            {
                CompleteQuest();
            }
        }

        private void CompleteQuest()
        {
            worldPlaceVisitor.OnVisitStarted -= OnPlaceVisited;
            callback?.Invoke();
        }
    }
}