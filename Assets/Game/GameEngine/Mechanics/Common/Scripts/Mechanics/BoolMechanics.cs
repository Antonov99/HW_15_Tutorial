using System;
using Elementary;
using Declarative;

namespace Game.GameEngine.Mechanics
{
    public sealed class BoolMechanics :
        IAwakeListener,
        IEnableListener,
        IDisableListener
    {
        private IVariable<bool> variable;

        private Action<bool> action;

        public void Construct(IVariable<bool> variable, Action<bool> action)
        {
            this.variable = variable;
            this.action = action;
        }

        void IAwakeListener.Awake()
        {
            action(variable.Current);
        }

        void IEnableListener.OnEnable()
        {
            variable.OnValueChanged += action;
        }

        void IDisableListener.OnDisable()
        {
            variable.OnValueChanged -= action;
        }
    }
}