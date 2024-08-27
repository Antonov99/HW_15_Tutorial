using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Elementary
{
    [Serializable]
    public sealed class Countdown : ICountdown, ISerializationCallbackReceiver
    {
        public event Action OnStarted;
        public event Action OnTimeChanged;
        public event Action OnStopped;
        public event Action OnEnded;
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
            get { return 1 - remainingTime / duration; }
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
        public float RemainingTime
        {
            get { return remainingTime; }
            set { remainingTime = Mathf.Clamp(value, 0, duration); }
        }
        
        [Space]
        [SerializeField]
        private float duration;

        private float remainingTime;

        private Coroutine coroutine;

        public Countdown()
        {
        }

        public Countdown(float duration)
        {
            this.duration = duration;
            remainingTime = duration;
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
                coroutine = null;
            }

            if (IsPlaying)
            {
                IsPlaying = false;
                OnStopped?.Invoke();
            }
        }

        public void ResetTime()
        {
            remainingTime = duration;
            OnReset?.Invoke();
        }

        private IEnumerator TimerRoutine()
        {
            while (remainingTime > 0)
            {
                yield return null;
                remainingTime -= Time.deltaTime;
                OnTimeChanged?.Invoke();
            }

            IsPlaying = false;
            OnEnded?.Invoke();
        }

        private void SetProgress(float progress)
        {
            progress = Mathf.Clamp01(progress);
            remainingTime = duration * (1 - progress);
            OnTimeChanged?.Invoke();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            remainingTime = duration;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }
    }
}