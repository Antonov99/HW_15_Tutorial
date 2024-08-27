using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Elementary
{
    [AddComponentMenu("Elementary/Variables/Variable «Bool»")]
    public sealed class MonoBoolVariable : MonoBehaviour, IVariable<bool>
    {
        public event Action<bool> OnValueChanged;

        public bool Current
        {
            get { return value; }
            set { SetValue(value); }
        }

        private readonly List<IAction<bool>> listeners = new();

        [OnValueChanged("SetValue")]
        [SerializeField]
        private bool value;

        [SerializeField]
        private UnityEvent<bool> onValueChanged;

        public void SetValue(bool value)
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

        public void SetTrue()
        {
            SetValue(true);
        }

        public void SetFalse()
        {
            SetValue(false);
        }

        public void AddListener(IAction<bool> listener)
        {
            listeners.Add(listener);
        }

        public void RemoveListener(IAction<bool> listener)
        {
            listeners.Remove(listener);
        }
    }
}