using System;
using Game.Gameplay.Player;

namespace Game.Tutorial
{
    public sealed class ConvertResourceInspector
    {
        private ConveyorVisitInputZoneObserver conveyorVisitInputZoneObserver;

        private Action callback;
        
        public void Construct(ConveyorVisitInputZoneObserver conveyorVisitInputZoneObserver)
        {
            this.conveyorVisitInputZoneObserver = conveyorVisitInputZoneObserver;
        }

        public void Inspect(Action callback)
        {
            this.callback = callback;
            conveyorVisitInputZoneObserver.OnResourcesLoaded += OnResourcesLoaded;
        }

        private void OnResourcesLoaded()
        {
            CompleteQuest();
        }

        private void CompleteQuest()
        {
            conveyorVisitInputZoneObserver.OnResourcesLoaded -= OnResourcesLoaded;
            callback?.Invoke();
        }
    }
}