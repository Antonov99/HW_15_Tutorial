using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public sealed class AmountDialog : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI amountText;

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private GameObject root;
        
        public void SetAmount(string amount)
        {
            amountText.text = amount;
        }

        public void SetIcon(Sprite icon)
        {
            iconImage.sprite = icon;
        }

        public void Show()
        {
            root.SetActive(true);
        }

        public void Hide()
        {
            root.SetActive(false);
        }
    }
}