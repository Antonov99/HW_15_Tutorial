using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Meta
{
    public sealed class MissionProgressBar : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private Color completeTextColor;

        [SerializeField]
        private Color processingTextColor;

        [Space]
        [SerializeField]
        private Image fill;

        [SerializeField]
        private Color completeFillColor;

        [SerializeField]
        private Color progressFillColor;
        
        public void SetProgress(float progress, string text)
        {
            fill.fillAmount = progress;
            this.text.text = text;

            if (progress >= 1)
            {
                this.text.color = completeTextColor;
                fill.color = completeFillColor;
            }
            else
            {
                this.text.color = processingTextColor;
                fill.color = progressFillColor;
            }
        }
    }
}