using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Elementary
{
    [AddComponentMenu("Elementary/Emitters/Emitter")]
    public sealed class MonoEmitter : MonoBehaviour, IEmitter
    {
        public event Action OnEvent;
        
        [SerializeField]
        private UnityEvent onEvent;
        
        private ActionComposite actions;

        [Button, GUIColor(0, 1, 0)]
        public void Call()
        {
            actions?.Do();
            onEvent.Invoke();
            OnEvent?.Invoke();
        }

        public void AddListener(IAction listener)
        {
            actions += listener;
        }

        public void RemoveListener(IAction listener)
        {
            actions -= listener;
        }
    }

    public abstract class MonoEmitter<T> : MonoBehaviour, IEmitter<T>
    {
        public event Action<T> OnEvent;

        [SerializeField]
        private UnityEvent<T> onEvent;

        private ActionComposite<T> actions;

        [Button, GUIColor(0, 1, 0)]
        public void Call(T value)
        {
            actions?.Do(value);
            onEvent?.Invoke(value);
            OnEvent?.Invoke(value);
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