using Windows;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Meta
{
    public sealed class DialoguePopup : MonoWindow<IDialoguePresentationModel>
    {
        [Space]
        [SerializeField]
        private UnityEvent onFinished;
        
        [Space]
        [SerializeField]
        private TextMeshProUGUI messageText;

        [SerializeField]
        private Image icon;

        [Space]
        [SerializeField]
        private DialogueOptionView[] optionViews;

        private IDialoguePresentationModel presentationModel;
        
        protected override void OnShow(IDialoguePresentationModel presentationModel)
        {
            this.presentationModel = presentationModel;
            this.presentationModel.OnStateChanged += OnStateChanged;
            this.presentationModel.OnFinished += OnFinished;

            icon.sprite = this.presentationModel.Icon;

            UpdateMessage();
            UpdateOptions();
        }

        protected override void OnHide()
        {
            ResetOptions();
        }

        private void OnFinished()
        {
            onFinished.Invoke();
        }

        private void OnStateChanged()
        {
            UpdateMessage();
            UpdateOptions();
        }

        private void UpdateMessage()
        {
            messageText.text = presentationModel.CurrentMessage;
        }

        private void UpdateOptions()
        {
            var options = presentationModel.CurrentOptions;
            var count = options.Length;

            for (var i = 0; i < count; i++)
            {
                var option = options[i];
                var view = optionViews[i];
                view.SetVisible(true);
                view.SetText(option.Text);
                view.SetClickAction(option.OnSelected);
            }

            for (int i = count, length = optionViews.Length; i < length; i++)
            {
                var view = optionViews[i];
                view.SetVisible(false);
                view.ClearClickAction();
            }
        }
        
        private void ResetOptions()
        {
            for (int i = 0, count = optionViews.Length; i < count; i++)
            {
                var view = optionViews[i];
                view.ClearClickAction();
            }
        }
    }
}