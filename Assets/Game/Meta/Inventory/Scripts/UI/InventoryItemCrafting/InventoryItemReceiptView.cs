using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Meta
{
    public sealed class InventoryItemReceiptView : MonoBehaviour
    {
        public event UnityAction OnCraftButtonClicked
        {
            add { craftButton.AddListener(value); }
            remove { craftButton.RemoveListener(value); }
        }

        public int IngredientCount
        {
            get { return ingredients.Length; }
        }

        [SerializeField]
        private TextMeshProUGUI titleText;

        [SerializeField]
        private TextMeshProUGUI descriptionText;

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private CraftButton craftButton;

        [Space]
        [SerializeField]
        private InventoryItemIngredientView[] ingredients;

        public void SetTitle(string title)
        {
            titleText.text = title;
        }

        public void SetDescription(string description)
        {
            descriptionText.text = description;
        }

        public void SetInteractibleButton(bool interactible)
        {
            var state = interactible
                ? CraftButton.State.AVAILABLE
                : CraftButton.State.LOCKED;
            craftButton.SetState(state);
        }

        public void SetIcon(Sprite icon)
        {
            iconImage.sprite = icon;
        }

        public InventoryItemIngredientView GetIngredient(int index)
        {
            return ingredients[index];
        }
    }
}