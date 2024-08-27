using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public sealed class NumberItem : MonoBehaviour
    {
        public int CurrentValue
        {
            get { return currentValue; }
        }

        [Header("Icon")]
        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private RectTransform iconCenterPoint;

        [Header("Text")]
        [LabelText("Use Text Mesh Pro")]
        [SerializeField]
        private bool textMeshPro;

        [HideIf("textMeshPro")]
        [LabelText("Count Text")]
        [SerializeField]
        private Text numberText;

        [ShowIf("textMeshPro")]
        [LabelText("Count Text")]
        [SerializeField]
        private TextMeshProUGUI numberTMP;

        [PropertyOrder(-10)]
        [PropertySpace]
        [ReadOnly]
        [ShowInInspector]
        private int currentValue;

        public Vector3 GetIconCenter()
        {
            return iconCenterPoint.position;
        }

        public void SetIcon(Sprite icon)
        {
            iconImage.sprite = icon;
        }

        public void SetupNumber(int number)
        {
            currentValue = number;
            UpdateText();
        }

        public void UpdateNumber(int number)
        {
            currentValue = number;
            UpdateText();
        }

        public void IncrementNumber(int range)
        {
            range = Math.Max(0, range);
            currentValue += range;
            UpdateText();
            AnimateBounce();
        }

        private void AnimateBounce()
        {
            var transform = iconImage.transform;
            DOTween.Sequence()
                .Append(transform.DOScale(new Vector3(1.1f, 1.1f, 1.0f), 0.1f))
                .Append(transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.15f));
        }

        public void DecrementNumber(int range)
        {
            range = Math.Max(0, range);
            currentValue -= range;
            UpdateText();
        }

        private void UpdateText()
        {
            var number = Math.Max(currentValue, 0);
            var numberString = number.ToString();

            if (textMeshPro)
                numberTMP.text = numberString;
            else
                numberText.text = numberString;
        }
    }
}