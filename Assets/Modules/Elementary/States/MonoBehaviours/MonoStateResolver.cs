using System;
using System.Collections.Generic;
using UnityEngine;

namespace Elementary
{
    public sealed class MonoStateResolver<T> : MonoBehaviour
    {
        [SerializeField]
        private MonoStateMachine<T> stateMachine;

        [SerializeField]
        private List<Transition> orderedTransitions;

        private void Update()
        {
            for (int i = 0, count = orderedTransitions.Count; i < count; i++)
            {
                var transition = orderedTransitions[i];
                if (transition.IsAvailable())
                {
                    stateMachine.SwitchState(transition.state);
                    return;
                }
            }
        }
        
        [Serializable]
        private struct Transition
        {
            [SerializeField]
            public MonoCondition[] conditions;

            [SerializeField]
            public T state;

            public bool IsAvailable()
            {
                for (int i = 0, count = conditions.Length; i < count; i++)
                {
                    var condition = conditions[i];
                    if (!condition.IsTrue())
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}