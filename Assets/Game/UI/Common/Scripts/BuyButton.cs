using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI
{
    public sealed class BuyButton : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        [Space]
        [SerializeField]
        private Image buttonBackground;

        [SerializeField]
        private Sprite availableButtonSprite;

        [SerializeField]
        private Sprite lockedButtonSprite;

        [Space]
        [SerializeField]
        private TextMeshProUGUI priceText;

        [SerializeField]
        private Image priceIcon;

        [Space]
        [SerializeField]
        private State state;

        public void AddListener(UnityAction action)
        {
            button.onClick.AddListener(action);
        }

        public void RemoveListener(UnityAction action)
        {
            button.onClick.RemoveListener(action);
        }

        public void SetPrice(string price)
        {
            priceText.text = price;
        }

        public void SetIcon(Sprite icon)
        {
            priceIcon.sprite = icon;
        }

        public void SetAvailable(bool isAvailable)
        {
            var state = isAvailable ? State.AVAILABLE : State.LOCKED;
            SetState(state);
        }

        public void SetState(State state)
        {
            this.state = state;

            if (state == State.AVAILABLE)
            {
                button.interactable = true;
                buttonBackground.sprite = availableButtonSprite;
            }
            else if (state == State.LOCKED)
            {
                button.interactable = false;
                buttonBackground.sprite = lockedButtonSprite;
            }
            else
            {
                throw new Exception($"Undefined button state {state}!");
            }
        }

        public enum State
        {
            AVAILABLE,
            LOCKED,
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            try
            {
                SetState(state);
            }
            catch (Exception)
            {
                // ignored
            }
        }
#endif
    }
}