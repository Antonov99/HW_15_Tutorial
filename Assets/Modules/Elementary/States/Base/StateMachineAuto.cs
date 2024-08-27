using System.Collections.Generic;

namespace Elementary
{
    public class StateMachineAuto<T> : StateMachine<T>
    {
        public List<StateTransition<T>> orderedTransitions;

        public void Update()
        {
            UpdateTransitions();
        }

        private void UpdateTransitions()
        {
            for (int i = 0, count = orderedTransitions.Count; i < count; i++)
            {
                var transition = orderedTransitions[i];
                if (transition.condition.IsTrue())
                {
                    if (!transition.stateId.Equals(CurrentState))
                    {
                        SwitchState(transition.stateId);                        
                    }
                    return;
                }
            }
        }
    }
}