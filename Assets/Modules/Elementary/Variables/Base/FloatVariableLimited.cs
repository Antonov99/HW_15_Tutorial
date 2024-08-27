using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Elementary
{
    [Serializable]
    public sealed class FloatVariableLimited : IVariableLimited<float>
    {
        public event Action<float> OnValueChanged;

        public event Action<float> OnMaxValueChanged;

        public float Current
        {
            get { return currentValue; }
            set { SetValue(value); }
        }

        public float MaxValue
        {
            get { return maxValue; }
            set { SetMaxValue(value); }
        }

        public bool IsLimit
        {
            get { return currentValue >= maxValue; }
        }

        private ActionComposite<float> onValueChanged;

        private ActionComposite<float> onMaxValueChanged;

        [OnValueChanged("SetValue")]
        [SerializeField]
        private float currentValue;

        [OnValueChanged("SetMaxValue")]
        [SerializeField]
        private float maxValue;

        public void AddListener(IAction<float> listener)
        {
            onValueChanged += listener;
        }

        public void RemoveListener(IAction<float> listener)
        {
            onValueChanged -= listener;
        }

        public void AddMaxListener(IAction<float> listener)
        {
            onMaxValueChanged += listener;
        }

        public void RemoveMaxListener(IAction<float> listener)
        {
            onMaxValueChanged -= listener;
        }
        
        public IAction<float> AddListener(Action<float> listener)
        {
            var actionDelegate = new ActionDelegate<float>(listener);
            onValueChanged += actionDelegate;
            return actionDelegate;
        }

        public IAction<float> AddMaxListener(Action<float> listener)
        {
            var actionDelegate = new ActionDelegate<float>(listener);
            onMaxValueChanged += actionDelegate;
            return actionDelegate;
        }

        private void SetValue(float value)
        {
            value = Mathf.Clamp(value, 0, maxValue);
            currentValue = value;
            onValueChanged?.Do(value);
            OnValueChanged?.Invoke(value);
        }
        
        private void SetMaxValue(float value)
        {
            value = Math.Max(1, value);
            if (currentValue > value)
            {
                SetValue(value);
            }

            maxValue = value;
            onMaxValueChanged?.Do(value);
            OnMaxValueChanged?.Invoke(value);
        }
    }
}