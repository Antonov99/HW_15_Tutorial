using Elementary;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Damage/Damage Connector")]
    public sealed class UDamageConnector : MonoBehaviour
    {
        [FormerlySerializedAs("baseDamage")]
        [SerializeField]
        public MonoIntVariable baseValue;

        [SerializeField]
        public MonoFloatVariable multiplier;

        [FormerlySerializedAs("fullDamage")]
        [SerializeField]
        public MonoIntVariable fullValue;

        private void Awake()
        {
            UpdateDamage();
        }

        private void OnEnable()
        {
            baseValue.OnValueChanged += OnValueChanged;
            multiplier.OnValueChanged += OnMultiplierChanged;
        }

        private void OnDisable()
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
            fullValue.SetValue(newValue);
        }

        private void UpdateDamage()
        {
            var newDamage = EvaluateFullValue();
            fullValue.SetValue(newDamage);
        }

        private int EvaluateFullValue()
        {
            var damage = baseValue.Current * multiplier.Current;
            return Mathf.RoundToInt(damage);
        }
    }
}