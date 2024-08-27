using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public sealed class PricePanel : MonoBehaviour
    {
        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private TextMeshProUGUI priceText;

        public void SetIcon(Sprite icon)
        {
            iconImage.sprite = icon;
        }

        public void SetPrice(string price)
        {
            priceText.text = price;
        }
    }
}