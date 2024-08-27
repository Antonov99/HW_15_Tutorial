using UnityEngine;

namespace Elementary
{
    public abstract class MonoEventMechanics : MonoBehaviour
    {
        [SerializeField]
        private MonoEmitter receiver;

        protected virtual void OnEnable()
        {
            receiver.OnEvent += OnEvent;
        }

        protected virtual  void OnDisable()
        {
            receiver.OnEvent -= OnEvent;
        }

        protected abstract void OnEvent();
    }
}