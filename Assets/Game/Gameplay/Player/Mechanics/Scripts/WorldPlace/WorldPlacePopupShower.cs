using Game.GameEngine;
using GameSystem;

namespace Game.Gameplay.Player
{
    public sealed class WorldPlacePopupShower : IGameReadyElement, IGameFinishElement
    {
        private PopupManager popupManager;

        private WorldPlaceVisitInteractor visitInteractor;

        private WorldPlacePopupConfig config;

        private bool enabled = true;
        
        [GameInject]
        public void Construct(
            WorldPlaceVisitInteractor visitInteractor,
            PopupManager popupManager,
            WorldPlacePopupConfig config
        )
        {
            this.visitInteractor = visitInteractor;
            this.popupManager = popupManager;
            this.config = config;
        }

        public void SetEnable(bool enabled)
        {
            this.enabled = enabled;
        }
        
        void IGameReadyElement.ReadyGame()
        {
            visitInteractor.OnVisitStarted += OnVisitStarted;
        }

        void IGameFinishElement.FinishGame()
        {
            visitInteractor.OnVisitStarted -= OnVisitStarted;
        }

        private void OnVisitStarted(WorldPlaceType placeType)
        {
            if (!enabled)
            {
                return;
            }
            
            if (config.FindPopupName(placeType, out var popupName))
            {
                popupManager.ShowPopup(popupName);
            }
        }
    }
}