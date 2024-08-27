using System;
using Elementary;

namespace Game.GameEngine.Mechanics
{
    public sealed class Component_MeleeDamage : 
        IComponent_GetMeleeDamage,
        IComponent_SetMeleeDamage,
        IComponent_OnMeleeDamageChanged
    {
        public Component_MeleeDamage(IVariable<int> damage)
        {
            this.damage = damage;
        }

        public event Action<int> OnDamageChanged
        {
            add { damage.OnValueChanged += value; }
            remove { damage.OnValueChanged -= value; }
        }

        public int Damage
        {
            get { return damage.Current; }
        }

        private readonly IVariable<int> damage;

        public void SetDamage(int damage)
        {
            this.damage.Current = damage;
        }
    }
}