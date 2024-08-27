using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Elementary
{
    public abstract class MonoVariable<T> : MonoBehaviour, IVariable<T>
    {
        public event Action<T> OnValueChanged;

        public T Current
        {
            get { return value; }
            set { SetValue(value); }
        }

        [OnValueChanged("SetValue")]
        [SerializeField]
        private T value;

        [SerializeField]
        private UnityEvent<T> onValueChanged;

        private ActionComposite<T> actions; 
        
        public void SetValue(T value)
        {
            this.value = value;
            actions?.Do(value);
            onValueChanged?.Invoke(value);
            OnValueChanged?.Invoke(value);
        }

        public void AddListener(IAction<T> listener)
        {
            actions += listener;
        }

        public void RemoveListener(IAction<T> listener)
        {
            actions -= listener;
        }
    }
}