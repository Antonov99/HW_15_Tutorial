using System;
using UnityEngine;

namespace Elementary
{
    [AddComponentMenu("Elementary/Variables/Variable «Int Limited»")]
    public sealed class MonoIntVariableLimited : MonoBehaviour, IVariableLimited<int>
    {
        public event Action<int> OnValueChanged
        {
            add { source.OnValueChanged += value; }
            remove { source.OnValueChanged -= value; }
        }

        public event Action<int> OnMaxValueChanged
        {
            add { source.OnMaxValueChanged += value; }
            remove { source.OnMaxValueChanged -= value; }
        }

        public int Current
        {
            get { return source.Current; }
            set { source.Current = value; }
        }

        public int MaxValue
        {
            get { return source.MaxValue; }
            set { source.MaxValue = value; }
        }

        public bool IsLimit
        {
            get { return source.IsLimit; }
        }

        [SerializeField]
        private IntVariableLimited source = new();

        public void AddListener(IAction<int> listener)
        {
            source.AddListener(listener);
        }

        public void RemoveListener(IAction<int> listener)
        {
            source.RemoveListener(listener);
        }


        public void AddMaxListener(IAction<int> listener)
        {
            source.AddMaxListener(listener);
        }

        public void RemoveMaxListener(IAction<int> listener)
        {
            source.RemoveMaxListener(listener);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            source.MaxValue = Math.Max(1, source.MaxValue);
            source.Current = Mathf.Clamp(source.Current, 0, source.MaxValue);
        }
#endif
    }
}