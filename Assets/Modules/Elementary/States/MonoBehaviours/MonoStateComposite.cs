using System.Collections.Generic;
using UnityEngine;

namespace Elementary
{
    [AddComponentMenu("Elementary/States/State «Composite»")]
    public class MonoStateComposite : MonoState
    {
        [SerializeField]
        private List<MonoState> stateComponents;

        public override void Enter()
        {
            for (int i = 0, count = stateComponents.Count; i < count; i++)
            {
                var state = stateComponents[i];
                state.Enter();
            }
        }

        public override void Exit()
        {
            for (int i = 0, count = stateComponents.Count; i < count; i++)
            {
                var state = stateComponents[i];
                state.Exit();
            }
        }

        public void AddState(MonoState state)
        {
            stateComponents.Add(state);
        }

        public void RemoveState(MonoState state)
        {
            stateComponents.Remove(state);
        }
    }
}