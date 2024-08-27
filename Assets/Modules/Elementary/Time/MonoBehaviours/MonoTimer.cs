using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Elementary
{
    [AddComponentMenu("Elementary/Time/Timer")]
    public sealed class MonoTimer : MonoBehaviour, ITimer
    {
        public event Action OnStarted;

        public event Action OnTimeChanged;

        public event Action OnCanceled;

        public event Action OnFinished;

        public event Action OnReset;

        [ReadOnly]
        [ShowInInspector]
        [PropertyOrder(-10)]
        [PropertySpace(8)]
        public bool IsPlaying { get; private set; }

        [ReadOnly]
        [ShowInInspector]
        [PropertyOrder(-9)]
        [ProgressBar(0, 1)]
        public float Progress
        {
            get { return currentTime / duration; }
            set { SetProgress(value); }
        }

        public float Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        [ReadOnly]
        [ShowInInspector]
        [PropertyOrder(-8)]
        public float CurrentTime
        {
            get { return currentTime; }
            set { currentTime = Mathf.Clamp(value, 0, duration); }
        }

        [SerializeField]
        private float duration = 5.0f;

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

        private void SetProgress(float progress)
        {
            progress = Mathf.Clamp01(progress);
            currentTime = duration * progress;
            OnTimeChanged?.Invoke();
        }

        private IEnumerator TimerRoutine()
        {
            while (currentTime < duration)
            {
                yield return null;
                currentTime += Time.deltaTime;
                OnTimeChanged?.Invoke();
            }

            IsPlaying = false;
            OnFinished?.Invoke();
        }
    }
}