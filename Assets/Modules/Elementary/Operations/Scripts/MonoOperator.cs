using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Elementary
{
    public abstract class MonoOperator<T> : MonoBehaviour, IOperator<T>
    {
        public event Action<T> OnStarted;

        public event Action<T> OnStopped;

        [PropertySpace]
        [PropertyOrder(-10)]
        [ReadOnly]
        [ShowInInspector]
        public bool IsActive
        {
            get { return Current != null; }
        }

        [PropertyOrder(-9)]
        [ReadOnly]
        [ShowInInspector]
        public T Current { get; private set; }

        [SerializeField]
        private MonoCondition<T>[] preconditions;

        [SerializeField]
        private MonoAction<T>[] startActions;

        [SerializeField]
        private MonoAction<T>[] stopActions;

        public bool CanStart(T operation)
        {
            if (IsActive)
            {
                return false;
            }

            for (int i = 0, count = preconditions.Length; i < count; i++)
            {
                var condition = preconditions[i];
                if (!condition.IsTrue(operation))
                {
                    return false;
                }
            }

            return true;
        }

        [Title("Methods")]
        [Button]
        public void DoStart(T operation)
        {
            if (!CanStart(operation))
            {
                Debug.LogWarning("Can't start combat!", this);
                return;
            }

            for (int i = 0, count = startActions.Length; i < count; i++)
            {
                var action = startActions[i];
                action.Do(operation);
            }

            Current = operation;
            OnStarted?.Invoke(operation);
        }

        [Button]
        public void Stop()
        {
            if (!IsActive)
            {
                Debug.LogWarning("Combat is not started!", this);
                return;
            }

            var operation = Current;
            for (int i = 0, count = stopActions.Length; i < count; i++)
            {
                var action = stopActions[i];
                action.Do(operation);
            }

            Current = default;
            OnStopped?.Invoke(operation);
        }
    }
}