using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Meta
{
    public sealed class MissionView : MonoBehaviour
    {
        public MissionProgressBar ProgressBar
        {
            get { return progressBar; }
        }

        public MissionRewardButton RewardButton
        {
            get { return button; }
        }

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private TextMeshProUGUI titleText;

        [SerializeField]
        private TextMeshProUGUI difficultyText;

        [SerializeField]
        private MissionProgressBar progressBar;

        [SerializeField]
        private MissionRewardButton button;

        public void SetIcon(Sprite icon)
        {
            iconImage.sprite = icon;
        }

        public void SetTitle(string title)
        {
            titleText.text = title;
        }

        public void SetDifficulty(string difficulty)
        {
            difficultyText.text = difficulty.ToUpper();
        }
    }
}