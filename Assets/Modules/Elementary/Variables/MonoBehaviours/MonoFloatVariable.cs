using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Elementary
{
    [AddComponentMenu("Elementary/Variables/Variable «Float»")]
    public sealed class MonoFloatVariable : MonoBehaviour, IVariable<float>
    {
        public event Action<float> OnValueChanged;

        public float Current
        {
            get { return value; }
            set { SetValue(value); }
        }

        private readonly List<IAction<float>> listeners = new();

        [OnValueChanged("SetValue")]
        [SerializeField]
        private float value;
        
        [SerializeField]
        private UnityEvent<float> onValueChanged;

        public void SetValue(float value)
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

        public void Plus(float range)
        {
            var newValue = value + range;
            SetValue(newValue);
        }

        public void Minus(float range)
        {
            var newValue = value - range;
            SetValue(newValue);
        }

        public void Multiply(float multiplier)
        {
            var newValue = value * multiplier;
            SetValue(newValue);
        }

        public void Divide(float divider)
        {
            var newValue = value / divider;
            SetValue(newValue);
        }

        public void AddListener(IAction<float> listener)
        {
            listeners.Add(listener);
        }

        public void RemoveListener(IAction<float> listener)
        {
            listeners.Remove(listener);
        }
    }
}