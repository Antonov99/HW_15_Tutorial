using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.App
{
    public sealed class RealtimeClock : MonoBehaviour
    {
        public delegate void SleepDelegate(long sleepSeconds);

        public event SleepDelegate OnStarted;
        public event Action OnPaused;
        public event SleepDelegate OnResumed;
        public event Action OnEnded;
        
        private bool isActive;
        private bool isPaused;
        private long realtimeSeconds;
        private float realtimeSinceStartupCache;
        private float secondAcc;

        [PropertySpace]
        [ReadOnly]
        [ShowInInspector]
        public bool IsActive
        {
            get { return isActive; }
        }

        [ReadOnly]
        [ShowInInspector]
        public bool IsPaused
        {
            get { return isPaused; }
        }

        [ReadOnly]
        [ShowInInspector]
        public long RealtimeSeconds
        {
            get { return realtimeSeconds; }
        }

        [Title("Methods")]
        [Button]
        public void Play(long realtimeSeconds, long sleepSeconds = 0)
        {
            isActive = true;
            isPaused = false;
            this.realtimeSeconds = realtimeSeconds;

            sleepSeconds = Math.Max(sleepSeconds, 0);
            OnStarted?.Invoke(sleepSeconds);
        }

        [Button]
        public void End()
        {
            isActive = false;
            isPaused = false;
            OnEnded?.Invoke();
        }

        private void Update()
        {
            if (isActive && !isPaused)
            {
                UpdateTime(Time.deltaTime);
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (!isActive)
            {
                return;
            }

            if (pauseStatus)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }

        private void OnApplicationQuit()
        {
            End();
        }

        private void UpdateTime(float deltaTime)
        {
            secondAcc += deltaTime;
            if (secondAcc < 1)
            {
                return;
            }

            var seconds = (int) secondAcc;
            secondAcc -= seconds;
            realtimeSeconds += seconds;
        }

        [Button]
        private void Pause()
        {
            if (isPaused)
            {
                return;
            }

            realtimeSinceStartupCache = Time.realtimeSinceStartup;
            isPaused = true;
            OnPaused?.Invoke();
        }

        [Button]
        private void Resume()
        {
            if (!isPaused)
            {
                return;
            }

            var sleepSeconds = (long) (Time.realtimeSinceStartup - realtimeSinceStartupCache);
            realtimeSeconds += sleepSeconds;
            isPaused = false;
            OnResumed?.Invoke(sleepSeconds);
        }
    }
}