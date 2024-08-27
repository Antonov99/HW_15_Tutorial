using Elementary;
using Declarative;

namespace Game.GameEngine.Mechanics
{
    public sealed class MoveSpeedConnector :
        IAwakeListener,
        IEnableListener,
        IDisableListener
    {
        private IVariable<float> baseSpeed;

        private IVariable<float> multiplier;

        private IVariable<float> fullSpeed;

        public void Construct(
            IVariable<float> baseSpeed,
            IVariable<float> multiplier,
            IVariable<float> fullSpeed
        )
        {
            this.baseSpeed = baseSpeed;
            this.multiplier = multiplier;
            this.fullSpeed = fullSpeed;
        }

        void IAwakeListener.Awake()
        {
            UpdateSpeed();
        }

        void IEnableListener.OnEnable()
        {
            baseSpeed.OnValueChanged += OnStateChanged;
            multiplier.OnValueChanged += OnStateChanged;
        }

        void IDisableListener.OnDisable()
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
            fullSpeed.Current = newSpeed;
        }
    }
}