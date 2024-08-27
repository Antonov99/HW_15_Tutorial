using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Elementary
{
    public sealed class BecameVisibleScript : MonoBehaviour
    {
        public event Action OnVisible;

        public event Action OnInvisible;

        [PropertySpace]
        [ReadOnly]
        [ShowInInspector]
        public bool IsVisible
        {
            get { return isVisible; }
        }

        private bool isVisible;

        [SerializeField]
        private UnityEvent onBecameVisible;

        [SerializeField]
        private UnityEvent onBecameInvisible;

        private void OnBecameVisible()
        {
            isVisible = true;
            OnVisible?.Invoke();
        }

        private void OnBecameInvisible()
        {
            isVisible = false;
            OnInvisible?.Invoke();
        }
    }
}