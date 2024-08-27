using Elementary;
using UnityEngine;

namespace Game.GameEngine
{
    public abstract class UTimerMechanics : MonoBehaviour
    {
        [SerializeField]
        public MonoTimer timer;

        protected virtual void OnEnable()
        {
            timer.OnStarted += OnTimerStarted;
            timer.OnTimeChanged += OnTimeChanged;
            timer.OnFinished += OnTimerFinished;
            timer.OnCanceled += OnTimerCanceled;
        }

        protected virtual void OnDisable()
        {
            timer.OnStarted -= OnTimerStarted;
            timer.OnTimeChanged -= OnTimeChanged;
            timer.OnFinished -= OnTimerFinished;
            timer.OnCanceled -= OnTimerCanceled;
        }

        protected virtual void OnTimerStarted()
        {
        }

        protected virtual void OnTimeChanged()
        {
        }

        protected virtual void OnTimerFinished()
        {
        }

        protected virtual void OnTimerCanceled()
        {
        }
    }
}