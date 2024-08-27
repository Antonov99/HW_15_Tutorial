using Elementary;
using Declarative;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    public sealed class DamageConnector : IAwakeListener, IEnableListener, IDisableListener
    {
        public IVariable<int> baseValue;

        public IVariable<float> multiplier;

        public IVariable<int> fullValue;

        public void Construct(
            IVariable<int> baseValue,
            IVariable<float> multiplier,
            IVariable<int> fullValue
        )
        {
            this.baseValue = baseValue;
            this.multiplier = multiplier;
            this.fullValue = fullValue;
        }

        void IAwakeListener.Awake()
        {
            UpdateDamage();
        }

        void IEnableListener.OnEnable()
        {
            baseValue.OnValueChanged += OnValueChanged;
            multiplier.OnValueChanged += OnMultiplierChanged;
        }

        void IDisableListener.OnDisable()
        {
            baseValue.OnValueChanged -= OnValueChanged;
            multiplier.OnValueChanged -= OnMultiplierChanged;
        }

        private void OnMultiplierChanged(float _)
        {
            UpdateDamage();
        }

        private void OnValueChanged(int _)
        {
            var newValue = EvaluateFullValue();
            fullValue.Current = newValue;
        }

        private void UpdateDamage()
        {
            var newDamage = EvaluateFullValue();
            fullValue.Current = newDamage;
        }

        private int EvaluateFullValue()
        {
            var damage = baseValue.Current * multiplier.Current;
            return Mathf.RoundToInt(damage);
        }
    }
}