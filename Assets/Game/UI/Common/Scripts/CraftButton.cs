using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI
{
    public sealed class CraftButton : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        [Space]
        [SerializeField]
        private Image buttonBackground;

        [SerializeField]
        private Sprite availableButtonSprite;

        [SerializeField]
        private Sprite unavailableButtonSprite;

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
                buttonBackground.sprite = unavailableButtonSprite;
            }
            else
            {
                throw new Exception($"Undefined button state {state}!");
            }
        }

        public enum State
        {
            AVAILABLE,
            LOCKED
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