using System;
using UnityEngine;

namespace AI.Agents
{
    public abstract class Agent
    {
        public event Action OnStarted;

        public event Action OnStopped;

        public bool IsPlaying
        {
            get { return isPlaying; }
        }

        protected bool isPlaying;

        public void Play()
        {
            if (isPlaying)
            {
                Debug.LogWarning($"Agent {GetType().Name} is already playing!");
                return;
            }

            OnStart();
            isPlaying = true;
            OnStarted?.Invoke();
        }

        protected abstract void OnStart();

        protected abstract void OnStop();

        public void Stop()
        {
            if (!isPlaying)
            {
                Debug.LogWarning($"Agent {GetType().Name} is not playing!");
                return;
            }

            OnStop();
            isPlaying = false;
            OnStopped?.Invoke();
        }
    }
}