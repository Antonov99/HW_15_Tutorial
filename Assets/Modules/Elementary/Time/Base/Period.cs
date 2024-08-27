using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Elementary
{
    [Serializable]
    public sealed class Period : IPeriod
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

        [SerializeField]
        private float period;

        private Coroutine coroutine;

        public Period()
        {
        }

        public Period(float period)
        {
            this.period = period;
        }

        public void Play()
        {
            if (coroutine == null)
            {
                coroutine = MonoHelper.Instance.StartCoroutine(PeriodRoutine());
                OnStarted?.Invoke();
            }
        }

        public void Stop()
        {
            if (coroutine != null)
            {
                MonoHelper.Instance.StopCoroutine(coroutine);
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