using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.AI.Unity
{
    public abstract class UnityAgent : MonoBehaviour, IAgent
    {
        public event Action OnStarted;

        public event Action OnStopped;

        [ShowInInspector, ReadOnly]
        public bool IsPlaying
        {
            get { return isPlaying; }
        }

        protected bool isPlaying;

        [Button]
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

        [Button]
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

        protected abstract void OnStop();
    }
}