using Windows;
using Asyncoroutine;
using Game.Localization;
using Game.Tutorial.Gameplay;
using Game.Tutorial.UI;
using GameSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Tutorial
{
    [AddComponentMenu("Tutorial/Complete «Congratulations»")]
    public sealed class CongratulationsCompleteObserver : TutorialCompleteObserver
    {
        [SerializeField]
        private AssetReference popupPrefab;

        [SerializeField]
        private CongratulationsConfig config;

        [SerializeField]
        private float showPopupDelay = 0.5f;
        
        private PopupManager popupManager;

        public override void ConstructGame(GameContext context)
        {
            base.ConstructGame(context);
            popupManager = context.GetService<PopupManager>();
        }

        protected override void OnTutorialComplete()
        {
            ShowPopup();
        }

        private async void ShowPopup()
        {
            await new WaitForSeconds(showPopupDelay);

            var handle = this.popupPrefab.LoadAssetAsync<GameObject>();
            await handle.Task;
            var popupPrefab = handle.Result.GetComponent<MonoWindow>();
            
            var title = LocalizationManager.GetCurrentText(config.title);
            var description = LocalizationManager.GetCurrentText(config.description);
            
            var args = new CongratulationsArgs(title, description);
            popupManager.Show(popupPrefab, args);
        }
    }
}