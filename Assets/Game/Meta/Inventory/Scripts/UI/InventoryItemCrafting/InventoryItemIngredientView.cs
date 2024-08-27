using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Meta
{
    public sealed class InventoryItemIngredientView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI titleText;

        [Space]
        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Sprite completedIcon;

        [SerializeField]
        private Sprite uncompletedIcon;

        public void Setup(string title, int requiredCount, int actualCount)
        {
            titleText.text = $"{title}: {actualCount}/{requiredCount}";
            iconImage.sprite = actualCount >= requiredCount
                ? completedIcon
                : uncompletedIcon;
        }

        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }
}