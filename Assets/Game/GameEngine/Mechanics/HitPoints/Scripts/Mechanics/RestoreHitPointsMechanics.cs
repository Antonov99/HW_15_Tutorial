using Elementary;
using Declarative;

namespace Game.GameEngine.Mechanics
{
    public sealed class RestoreHitPointsMechanics : IEnableListener, IDisableListener
    {
        private readonly Countdown delay = new();

        private readonly Period period = new();

        private IHitPoints hitPoints;

        private IEmitter<TakeDamageArgs> takeDamageEmitter;

        private int restoreAtTime;

        public void SetRestoreAtTime(int value)
        {
            restoreAtTime = value;
        }

        public void SetDelay(float delay)
        {
            this.delay.Duration = delay;
        }

        public void SetPeriod(float period)
        {
            this.period.Duration = period;
        }

        public void Construct(IHitPoints hitPoints, IEmitter<TakeDamageArgs> takeDamageEmitter)
        {
            this.hitPoints = hitPoints;
            this.takeDamageEmitter = takeDamageEmitter;
        }

        void IEnableListener.OnEnable()
        {
            takeDamageEmitter.OnEvent += OnDamageTaken;
            delay.OnEnded += OnDelayEnded;
            period.OnPeriodEvent += OnRestoreHitPoints;
        }

        void IDisableListener.OnDisable()
        {
            takeDamageEmitter.OnEvent -= OnDamageTaken;
            delay.OnEnded -= OnDelayEnded;
            period.OnPeriodEvent -= OnRestoreHitPoints;
        }

        private void OnDamageTaken(TakeDamageArgs damageArgs)
        {
            if (hitPoints.Current <= 0)
            {
                return;
            }

            //Сброс задержки:
            delay.ResetTime();
            if (!delay.IsPlaying)
            {
                delay.Play();
            }

            //Сброс периода:
            period.Stop();
        }

        private void OnDelayEnded()
        {
            period.Play();
        }

        private void OnRestoreHitPoints()
        {
            hitPoints.Current += restoreAtTime;
            if (hitPoints.Current >= hitPoints.Max)
            {
                period.Stop();
            }
        }
    }
}