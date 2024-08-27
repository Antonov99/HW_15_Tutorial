using System;
using Game.GameEngine;
using Sirenix.OdinInspector;

namespace Game.Gameplay.Player
{
    public sealed class WorldPlaceVisitInteractor
    {
        public event Action<WorldPlaceType> OnVisitStarted;
        
        public event Action<WorldPlaceType> OnVisitEnded;
        
        [ShowInInspector, ReadOnly]
        public bool IsVisiting { get; private set; }

        [ShowInInspector, ReadOnly]
        public WorldPlaceType CurrentPlace { get; private set; }
        
        public void StartVisit(WorldPlaceType type)
        {
            if (IsVisiting)
            {
                throw new Exception("Other visit place is already started!");
            }

            IsVisiting = true;
            CurrentPlace = type;
            OnVisitStarted?.Invoke(type);
        }

        public void EndVisit()
        {
            if (!IsVisiting)
            {
                return;
            }

            IsVisiting = false;
            OnVisitEnded?.Invoke(CurrentPlace);
        }
    }
}