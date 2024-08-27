using System;
using GameSystem;
using Windows;
using Game.GameEngine;
using UnityEngine;

namespace Game.Tutorial.UI
{
    public sealed class PopupManager : MonoBehaviour, IWindow.Callback, IGameConstructElement
    {
        private GameContext gameContext;

        private InputStateManager inputManager;

        [SerializeField]
        private Transform rootTransform;

        private MonoWindow currentPopup;

        private Action callback;

        public void Show(MonoWindow prefab, object args = null, Action callback = null)
        {
            if (currentPopup != null)
            {
                return;
            }

            this.callback = callback;
            currentPopup = Instantiate(prefab, rootTransform);

            if (currentPopup.TryGetComponent(out IGameElement element))
            {
                gameContext.RegisterElement(element);
            }

            inputManager.SwitchState(InputStateId.LOCK);
            currentPopup.Show(args, callback: this);
        }

        void IWindow.Callback.OnClose(IWindow popup)
        {
            currentPopup.Hide();
            inputManager.SwitchState(InputStateId.BASE);

            if (currentPopup.TryGetComponent(out IGameElement element))
            {
                gameContext.UnregisterElement(element);
            }
            
            Destroy(currentPopup.gameObject);
            currentPopup = null;
            callback?.Invoke();
        }

        void IGameConstructElement.ConstructGame(GameContext context)
        {
            gameContext = context;
            inputManager = context.GetService<InputStateManager>();
        }
    }
}