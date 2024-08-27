using Game.UI;
using TMPro;
using Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Meta
{
    public sealed class InventoryItemPopup : MonoWindow<IInventoryItemPresentationModel>
    {
        [SerializeField]
        private TextMeshProUGUI titleText;
        
        [SerializeField]
        private TextMeshProUGUI decriptionText;
        
        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Button consumeButton;

        [Space]
        [SerializeField]
        private StackView stackView;

        private IInventoryItemPresentationModel presenter;
        
        protected override void OnShow(IInventoryItemPresentationModel presenter)
        {
            this.presenter = presenter;

            titleText.text = presenter.Title;
            decriptionText.text = presenter.Description;
            iconImage.sprite = presenter.Icon;

            SetupStackContainer(presenter);
            SetupConsumeButton(presenter);
            consumeButton.onClick.AddListener(OnConsumeButtonClicked);
        }

        protected override void OnHide()
        {
            consumeButton.onClick.RemoveListener(OnConsumeButtonClicked);
        }

        private void OnConsumeButtonClicked()
        {
            presenter.OnConsumeClicked();
        }

        private void SetupStackContainer(IInventoryItemPresentationModel presenter)
        {
            var isStackableItem = presenter.IsStackableItem();
            stackView.SetVisible(isStackableItem);
            
            if (isStackableItem)
            {
                presenter.GetStackInfo(out var current, out var size);
                stackView.SetAmount(current, size);
            }
        }

        private void SetupConsumeButton(IInventoryItemPresentationModel presenter)
        {
            var isConsumableItem = presenter.IsConsumableItem();
            consumeButton.gameObject.SetActive(isConsumableItem);
            if (isConsumableItem)
            {
                consumeButton.interactable = presenter.CanConsumeItem();
            }
        }
    }
}