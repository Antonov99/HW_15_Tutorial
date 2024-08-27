using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Meta
{
    public sealed class UpgradeButton : MonoBehaviour
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

        [SerializeField]
        private Sprite maxButtonSprite;

        [Space]
        [SerializeField]
        private TextMeshProUGUI priceText;

        [Space]
        [SerializeField]
        private GameObject maxTextGO;

        [SerializeField]
        private GameObject titleTextGO;

        [SerializeField]
        private GameObject priceContainer;

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

        public void SetState(State state)
        {
            this.state = state;

            if (state == State.AVAILABLE)
            {
                button.interactable = true;
                buttonBackground.sprite = availableButtonSprite;

                priceContainer.SetActive(true);
                titleTextGO.SetActive(true);
                maxTextGO.SetActive(false);
            }
            else if (state == State.LOCKED)
            {
                button.interactable = false;
                buttonBackground.sprite = lockedButtonSprite;

                priceContainer.SetActive(true);
                titleTextGO.SetActive(true);
                maxTextGO.SetActive(false);
            }
            else if (state == State.MAX)
            {
                button.interactable = false;
                buttonBackground.sprite = maxButtonSprite;

                priceContainer.SetActive(false);
                titleTextGO.SetActive(false);
                maxTextGO.SetActive(true);
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
            MAX
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