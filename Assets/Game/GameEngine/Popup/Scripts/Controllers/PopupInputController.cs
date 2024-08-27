using GameSystem;

namespace Game.GameEngine
{
    public sealed class PopupInputController : 
        IGameStartElement,
        IGameFinishElement
    {
        private PopupManager popupManager;

        private InputStateManager inputManager;
        
        public void Construct(PopupManager popupManager, InputStateManager inputManager)
        {
            this.popupManager = popupManager;
            this.inputManager = inputManager;
        }

        void IGameStartElement.StartGame()
        {
            popupManager.OnPopupShown += OnPopupShown;
            popupManager.OnPopupHidden += OnPopupHidden;
        }

        void IGameFinishElement.FinishGame()
        {
            popupManager.OnPopupShown -= OnPopupShown;
            popupManager.OnPopupHidden -= OnPopupHidden;
        }

        private void OnPopupShown(PopupName _)
        {
            inputManager.SwitchState(InputStateId.LOCK);
        }

        private void OnPopupHidden(PopupName _)
        {
            if (!popupManager.HasActivePopups)
            {
                inputManager.SwitchState(InputStateId.BASE);
            }
        }
    }
}