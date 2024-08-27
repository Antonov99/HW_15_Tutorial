using UnityEngine;

namespace Elementary
{
    public abstract class MonoPeriodMechanics : MonoBehaviour
    {
        [SerializeField]
        private MonoPeriod behaviour;

        protected virtual void OnEnable()
        {
            behaviour.OnStarted += OnStarted;
            behaviour.OnPeriodEvent += OnPeriodEvent;
            behaviour.OnStoped += OnStopped;
        }
        
        protected virtual void OnDisable()
        {
            behaviour.OnStarted -= OnStarted;
            behaviour.OnPeriodEvent -= OnPeriodEvent;
            behaviour.OnStoped -= OnStopped;
        }

        protected virtual void OnStarted()
        {
        }

        protected virtual void OnStopped()
        {
        }

        protected virtual void OnPeriodEvent()
        {
        }
    }
}