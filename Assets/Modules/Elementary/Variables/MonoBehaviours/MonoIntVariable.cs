using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Elementary
{
    [AddComponentMenu("Elementary/Variables/Variable «Int»")]
    public sealed class MonoIntVariable : MonoBehaviour, IVariable<int>
    {
        public event Action<int> OnValueChanged;

        public int Current
        {
            get { return value; }
            set { SetValue(value); }
        }

        private readonly List<IAction<int>> listeners = new();

        [OnValueChanged("SetValue")]
        [SerializeField]
        private int value;

        [SerializeField]
        private UnityEvent<int> onValueChanged;

        public void SetValue(int value)
        {
            for (int i = 0, count = listeners.Count; i < count; i++)
            {
                var listener = listeners[i];
                listener.Do(value);
            }

            this.value = value;
            onValueChanged?.Invoke(value);
            OnValueChanged?.Invoke(value);
        }
        
        public void Plus(int range)
        {
            var newValue = value + range;
            SetValue(newValue);
        }

        public void Minus(int range)
        {
            var newValue = value - range;
            SetValue(newValue);
        }

        public void Multiply(int multiplier)
        {
            var newValue = value * multiplier;
            SetValue(newValue);
        }

        public void Divide(int divider)
        {
            var newValue = value / divider;
            SetValue(newValue);
        }

        public void Increment()
        {
            var newValue = value + 1;
            SetValue(newValue);
        }

        public void Decrement()
        {
            var newValue = value - 1;
            SetValue(newValue);
        }

        public void AddListener(IAction<int> listener)
        {
            listeners.Add(listener);
        }

        public void RemoveListener(IAction<int> listener)
        {
            listeners.Remove(listener);
        }
    }
}