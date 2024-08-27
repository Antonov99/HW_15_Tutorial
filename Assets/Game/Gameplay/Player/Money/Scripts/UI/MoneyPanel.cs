using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Gameplay.Player
{
    public sealed class MoneyPanel : MonoBehaviour
    {
        [PropertyOrder(-10)]
        [ReadOnly]
        [ShowInInspector]
        public int Money { get; private set; }

        [Space]
        [SerializeField]
        private Text moneyText;

        [SerializeField]
        private RectTransform iconTransform;

        public void SetupMoney(int money)
        {
            Money = money;
            UpdateMoneyText();
        }

        public void IncrementMoney(int range)
        {
            var newAmount = Money + range;

            Money = newAmount;
            UpdateMoneyText();
            AnimateIncome();
        }

        public void DecrementMoney(int range)
        {
            var newAmount = Money - range;
            Money = newAmount;
            UpdateMoneyText();
        }

        private void UpdateMoneyText()
        {
            var money = Math.Max(Money, 0);
            moneyText.text = money.ToString();
        }

        public Vector3 GetIconPosition()
        {
            return iconTransform.TransformPoint(iconTransform.rect.center);
        }

        private void AnimateIncome()
        {
            var rootTransform = iconTransform;
            DOTween.Sequence()
                .Append(rootTransform.DOScale(new Vector3(1.1f, 1.1f, 1.0f), 0.1f))
                .Append(rootTransform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.2f));
        }
    }
}