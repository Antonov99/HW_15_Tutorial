using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Elementary
{
    [Serializable]
    public sealed class Timer : ITimer
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

        [Space]
        [SerializeField]
        private float duration;

        private float currentTime;

        private Coroutine coroutine;

        public Timer()
        {
        }

        public Timer(float duration)
        {
            this.duration = duration;
            currentTime = 0;
        }

        public void Play()
        {
            if (IsPlaying)
            {
                return;
            }

            IsPlaying = true;
            OnStarted?.Invoke();
            coroutine = MonoHelper.Instance.StartCoroutine(TimerRoutine());
        }

        public void Stop()
        {
            if (coroutine != null)
            {
                MonoHelper.Instance.StopCoroutine(coroutine);
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