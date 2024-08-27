using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Elementary
{
    [AddComponentMenu("Elementary/Time/Period")]
    public sealed class MonoPeriod : MonoBehaviour, IPeriod
    {
        public event Action OnStarted;

        public event Action OnPeriodEvent;

        public event Action OnStoped;

        [PropertyOrder(-10)]
        [PropertySpace]
        [ReadOnly]
        [ShowInInspector]
        public bool IsActive
        {
            get { return coroutine != null; }
        }

        public float Duration
        {
            get { return period; }
            set { period = value; }
        }

        private Coroutine coroutine;

        [Tooltip("Period in seconds")]
        [SerializeField]
        private float period = 1.0f;

        public void Play()
        {
            if (coroutine == null)
            {
                coroutine = StartCoroutine(PeriodRoutine());
                OnStarted?.Invoke();
            }
        }

        public void Stop()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
                OnStoped?.Invoke();
            }
        }

        private IEnumerator PeriodRoutine()
        {
            var period = new WaitForSeconds(this.period);
            while (true)
            {
                yield return period;
                OnPeriodEvent?.Invoke();
            }
        }
    }
}