using System;
using Elementary;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Enable/Component «Enable»")]
    public sealed class UComponent_Enable : MonoBehaviour, IComponent_Enable
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

        public void SetEnable(bool isEnable)
        {
            this.isEnable.SetValue(isEnable);
        }

        [SerializeField]
        private MonoBoolVariable isEnable;
    }
}