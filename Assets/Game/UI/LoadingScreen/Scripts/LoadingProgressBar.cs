using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public sealed class LoadingProgressBar : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text text;

        [SerializeField]
        private Image fill;

        public void SetProgress(float progress)
        {
            fill.fillAmount = progress;
            text.text = $"{(progress * 100):F0}%";
        }
    }
}