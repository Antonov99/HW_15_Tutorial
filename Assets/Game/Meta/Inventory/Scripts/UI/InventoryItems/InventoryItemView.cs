using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Meta
{
    public sealed class InventoryItemView : MonoBehaviour
    {
        public StackView Stack
        {
            get { return stackView; }
        }

        [SerializeField]
        private TextMeshProUGUI titleText;

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Button button;

        [SerializeField]
        private StackView stackView;

        public void SetTitle(string title)
        {
            titleText.text = title;
        }

        public void SetIcon(Sprite icon)
        {
            iconImage.sprite = icon;
        }

        public void AddClickListener(UnityAction action)
        {
            button.onClick.AddListener(action);
        }

        public void RemoveClickListener(UnityAction action)
        {
            button.onClick.RemoveListener(action);
        }
    }
}