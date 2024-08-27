using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Meta
{
    public sealed class MissionRewardButton : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        [Space]
        [SerializeField]
        private Image buttonBackground;

        [SerializeField]
        private Sprite availableBackground;

        [SerializeField]
        private Sprite unavailableBackground;

        [Space]
        [SerializeField]
        private GameObject processingText;

        [SerializeField]
        private GameObject getText;

        [SerializeField]
        private TextMeshProUGUI rewardText;

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

        public void SetReward(string reward)
        {
            rewardText.text = reward;
        }

        public void SetState(State state)
        {
            this.state = state;

            if (state == State.COMPLETE)
            {
                button.interactable = true;
                buttonBackground.sprite = availableBackground;
                getText.SetActive(true);
                processingText.SetActive(false);
            }
            else if (state == State.PROCESSING)
            {
                button.interactable = false;
                buttonBackground.sprite = unavailableBackground;
                getText.SetActive(false);
                processingText.SetActive(true);
            }
            else
            {
                throw new Exception($"Undefined button state {state}!");
            }
        }

        public enum State
        {
            COMPLETE = 0,
            PROCESSING = 1
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