using Elementary;
using Declarative;

namespace Game.GameEngine.Mechanics
{
    public abstract class StateUpdate : State, IUpdateListener
    {
        private bool enabled;

        public override void Enter()
        {
            enabled = true;
        }

        public override void Exit()
        {
            enabled = false;
        }

        void IUpdateListener.Update(float deltaTime)
        {
            if (enabled)
            {
                Update(deltaTime);
            }
        }

        protected abstract void Update(float deltaTime);
    }
}