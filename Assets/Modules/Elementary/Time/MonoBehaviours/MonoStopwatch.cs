using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Elementary
{
    [AddComponentMenu("Elementary/Time/Stopwatch")]
    public sealed class MonoStopwatch : MonoBehaviour, IStopwatch
    {
        public event Action OnStarted;

        public event Action OnTimeChanged;

        public event Action OnCanceled;

        public event Action OnReset;

        [ReadOnly]
        [ShowInInspector]
        [PropertyOrder(-10)]
        [PropertySpace(8)]
        public bool IsPlaying { get; private set; }

        [ReadOnly]
        [ShowInInspector]
        [PropertyOrder(-8)]
        public float CurrentTime
        {
            get { return currentTime; }
            set { currentTime = Mathf.Max(value, 0); }
        }

        private float currentTime;

        private Coroutine coroutine;
        
        public void Play()
        {
            if (IsPlaying)
            {
                return;
            }

            IsPlaying = true;
            OnStarted?.Invoke();
            coroutine = StartCoroutine(TimerRoutine());
        }

        public void Stop()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            if (IsPlaying)
            {
                IsPlaying = false;
                OnCanceled?.Invoke();
            }
        }
        
        public void ResetTime()
        {
            currentTime = 0;
            OnReset?.Invoke();
        }

        private IEnumerator TimerRoutine()
        {
            while (true)
            {
                yield return null;
                currentTime += Time.deltaTime;
                OnTimeChanged?.Invoke();
            }
        }
    }
}