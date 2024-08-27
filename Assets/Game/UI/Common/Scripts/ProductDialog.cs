using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public sealed class ProductDialog : MonoBehaviour
    {
        [SerializeField]
        private GameObject root;
        
        [SerializeField]
        private TextMeshProUGUI priceText;

        [SerializeField]
        private Image currencyIconImage;

        [SerializeField]
        private Image productIconImage;

        public void Show()
        {
            root.SetActive(true);
        }

        public void Hide()
        {
            root.SetActive(false);
        }

        public void SetPrice(string price)
        {
            priceText.text = price;
        }

        public void SetProductIcon(Sprite icon)
        {
            productIconImage.sprite = icon;
        }

        public void SetCurrencyIcon(Sprite icon)
        {
            currencyIconImage.sprite = icon;
        }
    }
}