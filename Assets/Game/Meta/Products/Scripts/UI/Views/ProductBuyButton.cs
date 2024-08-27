using System;
using Game.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Meta
{
    public sealed class ProductBuyButton : MonoBehaviour
    {
        public event UnityAction OnClicked
        {
            add { button.onClick.AddListener(value); }
            remove { button.onClick.RemoveListener(value); }
        }

        public PricePanel PriceItem1
        {
            get { return priceItem1; }
        }

        public PricePanel PriceItem2
        {
            get { return priceItem2; }
        }
        
        [SerializeField]
        private RectTransform mainTransform;

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
        private PricePanel priceItem1;

        [SerializeField]
        private PricePanel priceItem2;

        [Space]
        [SerializeField]
        private State state;

        public void SetPriceSize1()
        {
            priceItem1.gameObject.SetActive(true);
            priceItem2.gameObject.SetActive(false);
            mainTransform.sizeDelta = new Vector2(mainTransform.sizeDelta.x, 140);
        }

        public void SetPriceSize2()
        {
            priceItem1.gameObject.SetActive(true);
            priceItem2.gameObject.SetActive(true);
            mainTransform.sizeDelta = new Vector2(mainTransform.sizeDelta.x, 200);
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