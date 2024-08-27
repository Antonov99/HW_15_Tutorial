using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    public abstract class UDestroyMechanics : MonoBehaviour
    {
        [SerializeField]
        public DestroyEventReceiver eventReceiver;

        protected virtual void OnEnable()
        {
            eventReceiver.OnDestroy += OnDestroyEvent;
        }

        protected virtual void OnDisable()
        {
            eventReceiver.OnDestroy -= OnDestroyEvent;
        }

        protected abstract void OnDestroyEvent(DestroyArgs destroyArgs);
    }
}