using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AI.Commands
{
    public abstract class UnityAICommandExecutor<T> : MonoBehaviour, IAICommandExecutor<T>
    {
        public event Action<T, object> OnStarted
        {
            add { executor.OnStarted += value; }
            remove { executor.OnStarted -= value; }
        }

        public event Action<T, object> OnFinished
        {
            add { executor.OnFinished += value; }
            remove { executor.OnFinished -= value; }
        }

        public event Action<T, object> OnInterrupted
        {
            add { executor.OnInterrupted += value; }
            remove { executor.OnInterrupted -= value; }
        }

        [ShowInInspector, ReadOnly]
        public bool IsRunning
        {
            get { return executor.IsRunning; }
        }

        protected readonly AICommandExecutor<T> executor = new();

        [Button]
        public void Execute(T key, object args = null)
        {
            executor.Execute(key, args);
        }

        [Button]
        public void Interrupt()
        {
            executor.Interrupt();
        }

        public bool TryGetRunningInfo(out T key, out object args)
        {
            return executor.TryGetRunningInfo(out key, out args);
        }

        public void RegisterCommand(T key, IAICommand command)
        {
            executor.RegisterCommand(key, command);
        }

        public void UnregisterCommand(T key)
        {
            executor.UnregisterCommand(key);
        }
    }
}