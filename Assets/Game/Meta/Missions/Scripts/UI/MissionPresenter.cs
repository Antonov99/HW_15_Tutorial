using System;
using Game.App;
using Game.Gameplay.Player;
using Game.Localization;
using Game.UI;
using UnityEngine;

namespace Game.Meta
{
    [Serializable]
    public sealed class MissionPresenter
    {
        public bool IsShown
        {
            get { return mission != null; }
        }

        [SerializeField]
        private MissionView view;

        private Mission mission;

        private MissionsManager missionsManager;

        private MoneyPanelAnimator_AddMoney moneyPanelAnimator;

        public void Construct(
            MissionsManager missionsManager,
            MoneyPanelAnimator_AddMoney moneyPanelAnimator
        )
        {
            this.missionsManager = missionsManager;
            this.moneyPanelAnimator = moneyPanelAnimator;
        }

        public void Start(Mission mission)
        {
            if (this.mission != null)
            {
                throw new Exception("Mission presenter is already shown!");
            }

            this.mission = mission;

            view.SetIcon(this.mission.Metadata.icon);
            view.gameObject.SetActive(true);

            SetupProgressBar();
            SetupRewardButton();

            view.RewardButton.AddListener(OnButtonClicked);
            this.mission.OnProgressChanged += OnMissionProgressChanged;
            this.mission.OnCompleted += OnMissionCompleted;

            LanguageManager.OnLanguageChanged += OnUpdateLanguage;
            OnUpdateLanguage(LanguageManager.CurrentLanguage);
        }

        public void Stop()
        {
            view.RewardButton.RemoveListener(OnButtonClicked);
            view.gameObject.SetActive(false);

            if (mission != null)
            {
                mission.OnProgressChanged -= OnMissionProgressChanged;
                mission.OnCompleted -= OnMissionCompleted;
                mission = null;
            }

            LanguageManager.OnLanguageChanged -= OnUpdateLanguage;
        }

        #region UIEvents

        private void OnButtonClicked()
        {
            var mission = this.mission;
            if (missionsManager.CanReceiveReward(mission))
            {
                missionsManager.ReceiveReward(mission);
                AnimateIncome(mission);
            }
        }

        #endregion

        #region ModelEvents

        private void OnMissionProgressChanged(Mission mission)
        {
            var progress = mission.NormalizedProgress;
            var text = mission.TextProgress;
            view.ProgressBar.SetProgress(progress, text);
        }

        private void OnMissionCompleted(Mission mission)
        {
            view.RewardButton.SetState(MissionRewardButton.State.COMPLETE);
        }

        private void OnUpdateLanguage(SystemLanguage language)
        {
            var title = LocalizationManager.GetText(mission.Metadata.localizedTitle, language);
            view.SetTitle(title);

            var difficultyKey = MissionExtensions.GetDifficultyLocalizationKey(mission.Difficulty);
            var difficultyText = LocalizationManager.GetText(difficultyKey, language);
            view.SetDifficulty(difficultyText);
        }

        #endregion

        private void SetupRewardButton()
        {
            var button = view.RewardButton;

            var reward = mission.MoneyReward.ToString();
            button.SetReward(reward);

            var state = mission.State == MissionState.COMPLETED
                ? MissionRewardButton.State.COMPLETE
                : MissionRewardButton.State.PROCESSING;
            button.SetState(state);
        }

        private void SetupProgressBar()
        {
            var progress = mission.NormalizedProgress;
            var text = mission.TextProgress;
            view.ProgressBar.SetProgress(progress, text);
        }

        private void AnimateIncome(Mission mission)
        {
            var rectTransform = view.RewardButton.GetComponent<RectTransform>();
            var startUIPosition = rectTransform.TransformPoint(rectTransform.rect.center);
            var reward = mission.MoneyReward;
            moneyPanelAnimator.PlayIncomeFromUI(startUIPosition, reward);
        }
    }
}