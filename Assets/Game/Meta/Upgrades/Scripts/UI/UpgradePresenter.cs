using Game.App;
using Game.Gameplay.Player;
using Game.Localization;
using UnityEngine;

namespace Game.Meta
{
    public sealed class UpgradePresenter
    {
        private const string UPGRADE_COLOR_HEX = "309D1E";

        private readonly Upgrade upgrade;

        private readonly UpgradeView view;

        private UpgradesManager upgradesManager;

        private MoneyStorage moneyStorage;

        public UpgradePresenter(Upgrade upgrade, UpgradeView view)
        {
            this.upgrade = upgrade;
            this.view = view;
        }

        public void Construct(UpgradesManager upgradesManager, MoneyStorage moneyStorage)
        {
            this.upgradesManager = upgradesManager;
            this.moneyStorage = moneyStorage;
        }

        public void Start()
        {
            view.SetIcon(upgrade.Metadata.icon);
            view.UpgradeButton.AddListener(OnButtonClicked);

            upgrade.OnLevelUp += OnLevelUp;
            moneyStorage.OnMoneyChanged += OnMoneyChanged;

            var language = LanguageManager.CurrentLanguage;
            UpdateTitle(language);
            UpdateLevel(language);
            UpdateStats(language);
            UpdateButtonPrice();
            UpdateButtonState();
            LanguageManager.OnLanguageChanged += OnUpdateLanguage;
        }

        public void Stop()
        {
            view.UpgradeButton.RemoveListener(OnButtonClicked);
            upgrade.OnLevelUp -= OnLevelUp;
            moneyStorage.OnMoneyChanged -= OnMoneyChanged;
            LanguageManager.OnLanguageChanged -= OnUpdateLanguage;
        }

        #region UIEvents

        private void OnButtonClicked()
        {
            if (upgradesManager.CanLevelUp(upgrade))
            {
                upgradesManager.LevelUp(upgrade);
            }
        }

        #endregion

        #region ModelEvents

        private void OnLevelUp(int level)
        {
            var language = LanguageManager.CurrentLanguage;
            UpdateLevel(language);
            UpdateStats(language);
            UpdateButtonPrice();
            UpdateButtonState();
        }

        private void OnMoneyChanged(int newValue)
        {
            UpdateButtonState();
        }

        private void OnUpdateLanguage(SystemLanguage language)
        {
            UpdateTitle(language);
            UpdateLevel(language);
            UpdateStats(language);
        }

        #endregion

        private void UpdateTitle(SystemLanguage language)
        {
            var titleKey = upgrade.Metadata.localizedTitle;
            var title = LocalizationManager.GetText(titleKey, language);
            view.SetTitle(title);
        }

        private void UpdateLevel(SystemLanguage language)
        {
            var title = LocalizationManager.GetText(LocalizationKeys.Common.LEVEL_KEY, language);
            var levelText = $"{title}: {upgrade.Level}/{upgrade.MaxLevel}";
            view.SetLevel(levelText);
        }
        
        private void UpdateStats(SystemLanguage language)
        {
            var title = LocalizationManager.GetText(LocalizationKeys.Common.VALUE_KEY, language);
            var statsText = $"{title}: {upgrade.CurrentStats}";
            
            if (!upgrade.IsMaxLevel)
            {
                statsText += $" <color=#{UPGRADE_COLOR_HEX}>(+{upgrade.NextImprovement})</color>";
            }
            
            view.SetStats(statsText);
        }

        private void UpdateButtonPrice()
        {
            var priceText = upgrade.NextPrice.ToString();
            view.UpgradeButton.SetPrice(priceText);
        }

        private void UpdateButtonState()
        {
            var upgradeButton = view.UpgradeButton;
            if (upgrade.IsMaxLevel)
            {
                upgradeButton.SetState(UpgradeButton.State.MAX);
                return;
            }

            var state = upgrade.NextPrice <= moneyStorage.Money
                ? UpgradeButton.State.AVAILABLE
                : UpgradeButton.State.LOCKED;

            upgradeButton.SetState(state);
        }
    }
}