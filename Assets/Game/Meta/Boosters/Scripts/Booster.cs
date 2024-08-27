using System;
using Elementary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Meta
{
    public abstract class Booster
    {
        public event Action<Booster> OnStarted;

        public event Action<Booster> OnTimeChanged;

        public event Action<Booster> OnStopped;

        public event Action<Booster> OnEnded;

        [ReadOnly]
        [ShowInInspector]
        public string Id
        {
            get { return config.id; }
        }

        [ReadOnly]
        [ShowInInspector]
        public bool IsActive
        {
            get { return timer.IsPlaying; }
        }

        [ReadOnly]
        [ShowInInspector]
        public float RemainingTime
        {
            get { return timer.RemainingTime; }
            set { timer.RemainingTime = value; }
        }

        [ReadOnly]
        [ShowInInspector]
        public float Duration
        {
            get { return timer.Duration; }
        }

        [ReadOnly]
        [ShowInInspector]
        [ProgressBar(0, 1)]
        public float Progress
        {
            get { return timer.Progress; }
        }

        [ReadOnly]
        [ShowInInspector]
        public BoosterMetadata Metadata
        {
            get { return config.metadata; }
        }

        private readonly BoosterConfig config;

        private readonly Countdown timer;

        public Booster(BoosterConfig config)
        {
            this.config = config;
            timer = new Countdown(config.duration);
        }

        public void Start()
        {
            if (timer.IsPlaying)
            {
                throw new Exception("Timer is already started!");
            }

            timer.OnTimeChanged += OnChangeTime;
            timer.OnEnded += OnEnd;

            OnStart();
            OnStarted?.Invoke(this);

            timer.Play();
        }

        public void Stop()
        {
            if (!timer.IsPlaying)
            {
                return;
            }

            timer.OnEnded -= OnEnd;
            timer.OnTimeChanged -= OnChangeTime;
            timer.Stop();

            OnStop();
            OnStopped?.Invoke(this);
        }

        protected abstract void OnStart();

        protected abstract void OnStop();

        private void OnEnd()
        {
            timer.OnEnded -= OnEnd;
            timer.OnTimeChanged -= OnChangeTime;

            OnStop();
            OnEnded?.Invoke(this);
        }

        private void OnChangeTime()
        {
            OnTimeChanged?.Invoke(this);
        }
    }
}