using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Elementary
{
    public sealed class Variable<T> : IVariable<T>
    {
        public event Action<T> OnValueChanged;

        public T Current
        {
            get { return value; }
            set { SetValue(value); }
        }
        
        [OnValueChanged("SetValue")]
        [ShowInInspector]
        private T value;
        
        private ActionComposite<T> onValueChanged;

        public void AddListener(IAction<T> listener)
        {
            onValueChanged += listener;
        }

        public void RemoveListener(IAction<T> listener)
        {
            onValueChanged -= listener;
        }

        public IAction<T> AddListener(Action<T> listener)
        {
            var actionDelegate = new ActionDelegate<T>(listener);
            onValueChanged += actionDelegate;
            return actionDelegate;
        }

        private void SetValue(T value)
        {
            this.value = value;
            onValueChanged?.Do(value);
            OnValueChanged?.Invoke(value);
        }
    }
}