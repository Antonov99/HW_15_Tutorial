using Elementary;
using Declarative;

namespace Game.GameEngine.Mechanics
{
    public abstract class TakeDamageMechanics : IEnableListener, IDisableListener
    {
        public IEmitter<TakeDamageArgs> emitter;

        void IEnableListener.OnEnable()
        {
            emitter.OnEvent += OnDamageTaken;
        }

        void IDisableListener.OnDisable()
        {
            emitter.OnEvent -= OnDamageTaken;
        }

        protected abstract void OnDamageTaken(TakeDamageArgs damageArgs);
    }
}