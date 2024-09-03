using System;
using Game.Gameplay.Player;

namespace Game.Tutorial
{
    public sealed class GetResourceInspector
    {
        private ConveyorVisitUnloadZoneObserver conveyorVisitUnloadZoneObserver;

        private Action callback;
        
        public void Construct(ConveyorVisitUnloadZoneObserver conveyorVisitUnloadZoneObserver)
        {
            this.conveyorVisitUnloadZoneObserver = conveyorVisitUnloadZoneObserver;
        }

        public void Inspect(Action callback)
        {
            this.callback = callback;
            conveyorVisitUnloadZoneObserver.OnResourcesUnloaded += OnResourcesUnloaded;
        }

        private void OnResourcesUnloaded()
        {
            CompleteQuest();
        }

        private void CompleteQuest()
        {
            conveyorVisitUnloadZoneObserver.OnResourcesUnloaded -= OnResourcesUnloaded;
            callback?.Invoke();
        }
    }
}