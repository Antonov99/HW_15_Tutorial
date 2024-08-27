using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Elementary
{
    public sealed class Effector<T> : IEffector<T>
    {
        public event Action<T> OnApplied;

        public event Action<T> OnDiscarded;

        [ShowInInspector, ReadOnly]
        private readonly List<T> effects = new();

        private readonly List<IEffectHandler<T>> handlers = new();

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

        public void AddHandler(IEffectHandler<T> handler)
        {
            handlers.Add(handler);
        }

        public void RemoveHandler(IEffectHandler<T> handler)
        {
            handlers.Remove(handler);
        }
    }
}