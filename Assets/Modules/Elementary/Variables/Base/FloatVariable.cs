using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Elementary
{
    [Serializable]
    public sealed class FloatVariable : IVariable<float>
    {
        public event Action<float> OnValueChanged;

        public float Current
        {
            get { return value; }
            set { SetValue(value); }
        }
        
        [OnValueChanged("SetValue")]
        [SerializeField]
        private float value;
        
        private ActionComposite<float> onValueChanged;

        public void AddListener(IAction<float> listener)
        {
            onValueChanged += listener;
        }

        public void RemoveListener(IAction<float> listener)
        {
            onValueChanged -= listener;
        }

        public IAction<float> AddListener(Action<float> listener)
        {
            var actionDelegate = new ActionDelegate<float>(listener);
            onValueChanged += actionDelegate;
            return actionDelegate;
        }

        private void SetValue(float value)
        {
            this.value = value;
            onValueChanged?.Do(value);
            OnValueChanged?.Invoke(value);
        }
    }
}