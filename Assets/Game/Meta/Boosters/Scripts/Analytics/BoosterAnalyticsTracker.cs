using GameSystem;

namespace Game.Meta
{
    public sealed class BoosterAnalyticsTracker : 
        IGameReadyElement,
        IGameFinishElement
    {
        private BoostersManager manager;

        [GameInject]
        public void Construct(BoostersManager manager)
        {
            this.manager = manager;
        }
        
        void IGameReadyElement.ReadyGame()
        {
            manager.OnBoosterLaunched += OnBoosterLaunched;
        }

        void IGameFinishElement.FinishGame()
        {
            manager.OnBoosterLaunched -= OnBoosterLaunched;
        }

        private void OnBoosterLaunched(Booster booster)
        {
            BoosterAnalytics.LogBoosterActivated(booster);
        }
    }
}