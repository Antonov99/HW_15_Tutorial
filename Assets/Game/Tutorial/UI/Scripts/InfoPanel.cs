using UnityEngine;
using UnityEngine.UI;

namespace Game.Tutorial.UI
{
    public sealed class InfoPanel : MonoBehaviour
    {
        [SerializeField]
        private Text titleText;

        [SerializeField]
        private Image iconImage;

        public void SetTitle(string title)
        {
            titleText.text = title;
        }

        public void SetIcon(Sprite icon)
        {
            iconImage.sprite = icon;
        }
    }
}