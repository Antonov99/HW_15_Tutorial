using System;
using Elementary;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    public sealed class Component_Enable : IComponent_Enable
    {
        public event Action<bool> OnEnabled
        {
            add { isEnable.OnValueChanged += value; }
            remove { isEnable.OnValueChanged -= value; }
        }

        public bool IsEnable
        {
            get { return isEnable.Current; }
        }

        private readonly IVariable<bool> isEnable;

        public Component_Enable(IVariable<bool> isEnable)
        {
            this.isEnable = isEnable;
        }

        public void SetEnable(bool isEnable)
        {
            this.isEnable.Current = isEnable;
        }
    }
}