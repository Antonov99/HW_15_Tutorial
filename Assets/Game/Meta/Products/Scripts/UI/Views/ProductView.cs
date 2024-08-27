using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Meta
{
    public sealed class ProductView : MonoBehaviour
    {
        public ProductBuyButton BuyButton
        {
            get { return button; }
        }

        [SerializeField]
        private ProductBuyButton button;
        
        [SerializeField]
        private TextMeshProUGUI titleText;

        [SerializeField]
        private TextMeshProUGUI descriptionText;

        [SerializeField]
        private Image iconImage;

        public void SetTitle(string title)
        {
            titleText.text = title;
        }

        public void SetDescription(string description)
        {
            descriptionText.text = description;
        }

        public void SetIcon(Sprite icon)
        {
            iconImage.sprite = icon;
        }
    }
}