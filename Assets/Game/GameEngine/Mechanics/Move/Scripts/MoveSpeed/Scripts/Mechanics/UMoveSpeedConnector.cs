using Elementary;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Move/Move Speed Connector")]
    public sealed class UMoveSpeedConnector : MonoBehaviour
    {
        [SerializeField]
        public MonoFloatVariable baseSpeed;

        [SerializeField]
        public MonoFloatVariable multiplier;

        [Space]
        [SerializeField]
        public MonoFloatVariable fullSpeed;

        private void Awake()
        {
            UpdateSpeed();
        }

        private void OnEnable()
        {
            baseSpeed.OnValueChanged += OnStateChanged;
            multiplier.OnValueChanged += OnStateChanged;
        }

        private void OnDisable()
        {
            baseSpeed.OnValueChanged -= OnStateChanged;
            multiplier.OnValueChanged -= OnStateChanged;
        }

        private void OnStateChanged(float _)
        {
           UpdateSpeed();
        }

        private void UpdateSpeed()
        {
            var newSpeed = baseSpeed.Current * multiplier.Current;
            fullSpeed.SetValue(newSpeed);
        }
    }
}