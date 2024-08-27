using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Elementary
{
    [Serializable]
    public sealed class BoolVariable : IVariable<bool>
    {
        public event Action<bool> OnValueChanged;
        
        public bool Current
        {
            get { return value; }
            set { SetValue(value); }
        }
        
        [OnValueChanged("SetValue")]
        [SerializeField]
        private bool value;
        
        private ActionComposite<bool> onValueChanged;

        public void AddListener(IAction<bool> listener)
        {
            onValueChanged += listener;
        }

        public void RemoveListener(IAction<bool> listener)
        {
            onValueChanged -= listener;
        }

        public IAction<bool> AddListener(Action<bool> listener)
        {
            var actionDelegate = new ActionDelegate<bool>(listener);
            onValueChanged += actionDelegate;
            return actionDelegate;
        }

        private void SetValue(bool value)
        {
            this.value = value;
            onValueChanged?.Do(value);
            OnValueChanged?.Invoke(value);
        }
    }
}