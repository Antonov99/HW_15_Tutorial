using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Meta
{
    public sealed class BoosterView : MonoBehaviour
    {
        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private TextMeshProUGUI timerText;

        [SerializeField]
        private TextMeshProUGUI labelText;

        [SerializeField]
        private ProgressBar progressBar;

        public void SetIcon(Sprite icon)
        {
            iconImage.sprite = icon;
        }

        public void SetColor(Color color)
        {
            progressBar.SetColor(color);
        }

        public void SetLabel(string label)
        {
            labelText.text = label;
        }

        public void SetProgress(float progress)
        {
            progressBar.SetProgress(progress);
        }
        
        public void SetRemainingText(string text)
        {
            timerText.text = text;
        }
    }
}