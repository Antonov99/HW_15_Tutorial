using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Elementary
{
    public abstract class MonoEffector<T> : MonoBehaviour, IEffector<T>
    {
        public event Action<T> OnApplied;

        public event Action<T> OnDiscarded;

        [Space]
        [ShowInInspector, ReadOnly]
        private List<T> effects = new();

        [Space]
        [SerializeField]
        private List<MonoEffectHandler<T>> handlers = new();

        [Title("Methods")]
        [Button]
        public void Apply(T effect)
        {
            for (var i = 0; i < handlers.Count; i++)
            {
                var handler = handlers[i];
                handler.OnApply(effect);
            }

            effects.Add(effect);
            OnApplied?.Invoke(effect);
        }

        [Button]
        public void Discard(T effect)
        {
            if (!effects.Remove(effect))
            {
                return;
            }

            for (var i = 0; i < handlers.Count; i++)
            {
                var handler = handlers[i];
                handler.OnDiscard(effect);
            }

            OnDiscarded?.Invoke(effect);
        }

        public bool IsExists(T effect)
        {
            return effects.Contains(effect);
        }

        public T[] GetEffects()
        {
            return effects.ToArray();
        }
    }
}