using Elementary;
using Declarative;

namespace Game.Gameplay.ResourceObjects
{
    public sealed class RespawnMechanics :
        IEnableListener,
        IDisableListener
    {
        private readonly Countdown countdown = new();
        
        private IEmitter destroyEvent;

        private IEmitter respawnEvent;

        public void ConstructDuration(float duration)
        {
            countdown.Duration = duration;
        }

        public void ConstructRespawnEvent(IEmitter respawnEvent)
        {
            this.respawnEvent = respawnEvent;
        }

        public void ConstructDestroyEvent(IEmitter destroyEvent)
        {
            this.destroyEvent = destroyEvent;
        }

        void IEnableListener.OnEnable()
        {
            destroyEvent.OnEvent += OnDeactivate;
            countdown.OnEnded += OnActivate;
        }

        void IDisableListener.OnDisable()
        {
            destroyEvent.OnEvent -= OnDeactivate;
            countdown.OnEnded -= OnActivate;
        }

        private void OnDeactivate()
        {
            countdown.ResetTime();
            countdown.Play();
        }

        private void OnActivate()
        {
            respawnEvent.Call();
        }
    }
}