using System;
using Elementary;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Damage/Component «Melee Damage»")]
    public sealed class UComponent_MeleeDamage : MonoBehaviour,
        IComponent_GetMeleeDamage,
        IComponent_SetMeleeDamage,
        IComponent_OnMeleeDamageChanged
    {
        public event Action<int> OnDamageChanged
        {
            add { damage.OnValueChanged += value; }
            remove { damage.OnValueChanged -= value; }
        }

        public int Damage
        {
            get { return damage.Current; }
        }

        [SerializeField]
        private MonoIntVariable damage;

        public void SetDamage(int damage)
        {
            this.damage.SetValue(damage);
        }
    }
}