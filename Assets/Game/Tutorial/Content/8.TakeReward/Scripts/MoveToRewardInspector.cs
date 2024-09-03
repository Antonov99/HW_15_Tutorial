using System;
using Game.GameEngine;
using Game.Gameplay.Player;
using UnityEngine;

namespace Game.Tutorial
{
    public sealed class MoveToRewardInspector
    {
        private WorldPlaceVisitInteractor worldPlaceVisitor;

        private RewardConfig config;

        private Action callback;

        public void Construct(WorldPlaceVisitInteractor worldPlaceVisitor, RewardConfig config)
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
            Debug.Log("visited");
            if (placeType == config.worldPlaceType)
            {
                Debug.Log("visited tavern");
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