using Elementary;

namespace Game.GameEngine.Mechanics
{
    public sealed class Component_CanCollect_BoolVariable : IComponent_CanCollect
    {
        public bool CanCollect
        {
            get { return isActive.Current; }
        }

        private readonly IVariable<bool> isActive;

        public Component_CanCollect_BoolVariable(IVariable<bool> isActive)
        {
            this.isActive = isActive;
        }
    }
}