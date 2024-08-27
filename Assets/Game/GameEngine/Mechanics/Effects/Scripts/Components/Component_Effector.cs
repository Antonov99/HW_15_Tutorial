using System;
using Elementary;

namespace Game.GameEngine.Mechanics
{
    public sealed class Component_Effector : IComponent_Effector
    {
        public event Action<IEffect> OnApplied
        {
            add { effector.OnApplied += value; }
            remove { effector.OnApplied -= value; }
        }

        public event Action<IEffect> OnDiscarded
        {
            add { effector.OnDiscarded += value; }
            remove { effector.OnDiscarded -= value; }
        }

        private readonly IEffector<IEffect> effector;

        public Component_Effector(IEffector<IEffect> effector)
        {
            this.effector = effector;
        }

        public void Apply(IEffect effect)
        {
            effector.Apply(effect);
        }

        public void Discard(IEffect effect)
        {
            effector.Discard(effect);
        }
    }
}