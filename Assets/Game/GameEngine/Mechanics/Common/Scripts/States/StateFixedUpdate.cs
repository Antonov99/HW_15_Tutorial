using Elementary;
using Declarative;

namespace Game.GameEngine.Mechanics
{
    public abstract class StateFixedUpdate : State, IFixedUpdateListener
    {
        private bool enabled;

        public sealed override void Enter()
        {
            OnEnter();
            enabled = true;
        }

        public sealed override void Exit()
        {
            enabled = false;
            OnExit();
        }

        void IFixedUpdateListener.FixedUpdate(float deltaTime)
        {
            if (enabled)
            {
                FixedUpdate(deltaTime);
            }
        }

        protected abstract void FixedUpdate(float deltaTime);

        protected virtual void OnEnter()
        {
        }

        protected virtual void OnExit()
        {
        }
    }
}