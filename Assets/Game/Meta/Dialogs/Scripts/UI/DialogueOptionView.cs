using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Meta
{
    public sealed class DialogueOptionView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI contentText;

        [SerializeField]
        private Button button;

        private UnityAction clickAction;

        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        public void SetText(string text)
        {
            contentText.text = text;
        }

        public void SetClickAction(UnityAction action)
        {
            ClearClickAction();
            clickAction = action;
            button.onClick.AddListener(action);
        }

        public void ClearClickAction()
        {
            if (clickAction != null)
            {
                button.onClick.RemoveListener(clickAction);
                clickAction = null;
            }
        }
    }
}