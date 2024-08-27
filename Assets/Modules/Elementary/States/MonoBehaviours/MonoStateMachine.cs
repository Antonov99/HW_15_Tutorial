using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Elementary
{
    public abstract class MonoStateMachine<T> : MonoState,
        IStateMachine<T>,
        ISerializationCallbackReceiver
    {
        public event Action<T> OnStateSwitched;

        public T CurrentState
        {
            get { return mode; }
        }

        [Space]
        [SerializeField]
        private bool enterOnEnable;

        [SerializeField]
        private bool exitOnDisable;

        [OnValueChanged("SwitchState")]
        [Space]
        [SerializeField]
        private T mode;

        [SerializeField]
        private StateHolder[] states = Array.Empty<StateHolder>();

        private Dictionary<T, MonoState> stateMap;

        private MonoState currentState;

        protected virtual void OnEnable()
        {
            if (enterOnEnable)
            {
                Enter();
            }
        }

        protected virtual void OnDisable()
        {
            if (exitOnDisable)
            {
                Exit();
            }
        }

        public virtual void SwitchState(T state)
        {
            if (!ReferenceEquals(currentState, null))
            {
                currentState.Exit();
            }

            if (stateMap.TryGetValue(state, out currentState))
            {
                currentState.Enter();
            }

            mode = state;
            OnStateSwitched?.Invoke(state);
        }

        public override void Enter()
        {
            if (ReferenceEquals(currentState, null) &&
                stateMap.TryGetValue(mode, out currentState))
            {
                currentState.Enter();
            }
        }

        public override void Exit()
        {
            if (!ReferenceEquals(currentState, null))
            {
                currentState.Exit();
                currentState = null;
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            stateMap = new Dictionary<T, MonoState>();
            foreach (var info in states)
            {
                stateMap[info.mode] = info.state;
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        [Serializable]
        private struct StateHolder
        {
            [SerializeField]
            public T mode;

            [SerializeField]
            public MonoState state;
        }
    }
}