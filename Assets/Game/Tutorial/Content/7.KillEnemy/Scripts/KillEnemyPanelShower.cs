using System;
using Game.App;
using Game.Localization;
using Game.Tutorial.Gameplay;
using Game.Tutorial.UI;
using UnityEngine;

namespace Game.Tutorial
{
    [Serializable]
    public sealed class KillEnemyPanelShower : InfoPanelShower
    {
        private KillEnemyConfig config;

        public void Construct(KillEnemyConfig config)
        {
            this.config = config;
        }

        protected override void OnShow()
        {
            var title = LocalizationManager.GetCurrentText(config.title);
            view.SetTitle(title);
            view.SetIcon(config.icon);

            LanguageManager.OnLanguageChanged += OnLanguageChanged;
        }

        protected override void OnHide()
        {
            LanguageManager.OnLanguageChanged -= OnLanguageChanged;
        }

        private void OnLanguageChanged(SystemLanguage language)
        {
            var title = LocalizationManager.GetText(config.title, language);
            view.SetTitle(title);
        }
    }
}