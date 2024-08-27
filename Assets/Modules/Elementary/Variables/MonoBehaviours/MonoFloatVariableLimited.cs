using System;
using UnityEngine;

namespace Elementary
{
    [AddComponentMenu("Elementary/Variables/Variable «Float Limited»")]
    public sealed class MonoFloatVariableLimited : MonoBehaviour, IVariableLimited<float>
    {
        public event Action<float> OnValueChanged
        {
            add { source.OnValueChanged += value; }
            remove { source.OnValueChanged -= value; }
        }

        public event Action<float> OnMaxValueChanged
        {
            add { source.OnMaxValueChanged += value; }
            remove { source.OnMaxValueChanged -= value; }
        }

        public float Current
        {
            get { return source.Current; }
            set { source.Current = value; }
        }

        public float MaxValue
        {
            get { return source.MaxValue; }
            set { source.MaxValue = value; }
        }

        public bool IsLimit
        {
            get { return source.IsLimit; }
        }

        [SerializeField]
        private FloatVariableLimited source = new();

        public void AddListener(IAction<float> listener)
        {
            source.AddListener(listener);
        }

        public void RemoveListener(IAction<float> listener)
        {
            source.RemoveListener(listener);
        }

        public void AddMaxListener(IAction<float> listener)
        {
            source.AddMaxListener(listener);
        }

        public void RemoveMaxListener(IAction<float> listener)
        {
            source.RemoveMaxListener(listener);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            source.MaxValue = Mathf.Max(1, source.MaxValue);
            source.Current = Mathf.Clamp(source.Current, 0, source.MaxValue);
        }
#endif
    }
}