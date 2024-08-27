using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Elementary
{
    [Serializable]
    public sealed class IntVariableLimited : IVariableLimited<int>
    {
        public event Action<int> OnValueChanged;

        public event Action<int> OnMaxValueChanged;

        public int Current
        {
            get { return currentValue; }
            set { SetValue(value); }
        }

        public int MaxValue
        {
            get { return maxValue; }
            set { SetMaxValue(value); }
        }

        public bool IsLimit
        {
            get { return currentValue >= maxValue; }
        }

        private ActionComposite<int> onValueChanged;

        private ActionComposite<int> onMaxValueChanged;

        [OnValueChanged("SetValue")]
        [SerializeField]
        private int currentValue;

        [OnValueChanged("SetMaxValue")]
        [SerializeField]
        private int maxValue;

        public void AddListener(IAction<int> listener)
        {
            onValueChanged += listener;
        }

        public void RemoveListener(IAction<int> listener)
        {
            onValueChanged -= listener;
        }

        public void AddMaxListener(IAction<int> listener)
        {
            onMaxValueChanged += listener;
        }

        public void RemoveMaxListener(IAction<int> listener)
        {
            onMaxValueChanged -= listener;
        }

        public IAction<int> AddListener(Action<int> listener)
        {
            var actionDelegate = new ActionDelegate<int>(listener);
            onValueChanged += actionDelegate;
            return actionDelegate;
        }

        public IAction<int> AddMaxListener(Action<int> listener)
        {
            var actionDelegate = new ActionDelegate<int>(listener);
            onMaxValueChanged += actionDelegate;
            return actionDelegate;
        }

        private void SetValue(int value)
        {
            value = Mathf.Clamp(value, 0, maxValue);
            currentValue = value;
            onValueChanged?.Do(value);
            OnValueChanged?.Invoke(value);
        }

        private void SetMaxValue(int value)
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