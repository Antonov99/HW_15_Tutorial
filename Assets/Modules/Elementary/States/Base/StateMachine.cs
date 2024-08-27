using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Elementary
{
    public class StateMachine<T> : State, IStateMachine<T>
    {
        public event Action<T> OnStateSwitched;

        public T CurrentState
        {
            get { return currentId; }
        }
        
        public List<StateEntry<T>> states = new();
        
        [OnValueChanged("SwitchState")]
        [Space, ShowInInspector, LabelText("Current State"), PropertyOrder(-10)]
        private T currentId;

        private IState currentState;

        public virtual void SwitchState(T key)
        {
            if (currentState != null)
            {
                currentState.Exit();
            }

            currentId = key;
            if (FindState(currentId, out currentState))
            {
                currentState.Enter();
            }

            OnStateSwitched?.Invoke(key);
        }

        [Title("Methods")]
        [Button, GUIColor(0, 1, 0)]
        public override void Enter()
        {
            if (currentState == null && FindState(currentId, out currentState))
            {
                currentState.Enter();
            }
        }

        [Button, GUIColor(0, 1, 0)]
        public override void Exit()
        {
            if (currentState != null)
            {
                currentState.Exit();
                currentState = null;
            }
        }

        public void AddState(T key, IState state)
        {
            var entry = new StateEntry<T>(key, state);
            states.Add(entry);
        }

        public void RemoveState(T key)
        {
            for (int i = 0, count = states.Count; i < count; i++)
            {
                var state = states[i];
                if (key.Equals(state.key))
                {
                    states.Remove(state);
                    return;
                }
            }
        }

        public void ClearStates()
        {
            states.Clear();
        }
        
        public static StateMachine<T> operator +(StateMachine<T> stateMachine, StateEntry<T> state)
        {
            stateMachine.states.Add(state);
            return stateMachine;
        }

        public static StateMachine<T> operator +(StateMachine<T> stateMachine, IEnumerable<StateEntry<T>> states)
        {
            stateMachine.states.AddRange(states);
            return stateMachine;
        }

        public static StateMachine<T> operator -(StateMachine<T> stateMachine, StateEntry<T> state)
        {
            stateMachine.states.Remove(state);
            return stateMachine;
        }

        private bool FindState(T type, out IState state)
        {
            for (int i = 0, count = states.Count; i < count; i++)
            {
                var holder = states[i];
                if (holder.key.Equals(type))
                {
                    state = holder.state;
                    return true;
                }
            }

            state = default;
            return false;
        }
    }
}