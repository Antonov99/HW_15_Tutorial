using Elementary;
using Declarative;

namespace Game.GameEngine.Mechanics
{
    public abstract class DestroyMechanics :
        IEnableListener,
        IDisableListener
    {
        public IEmitter<DestroyArgs> emitter;

        void IEnableListener.OnEnable()
        {
            emitter.OnEvent += Destroy;
        }

        void IDisableListener.OnDisable()
        {
            emitter.OnEvent -= Destroy;
        }

        protected abstract void Destroy(DestroyArgs destroyArgs);
    }
}