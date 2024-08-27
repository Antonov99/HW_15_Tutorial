using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Commands
{
    public abstract class UnityAICommandEnqueuer<T> : MonoBehaviour, IAICommandEnqueuer<T>
    {
        public event Action<T, object> OnEnqueued
        {
            add { enqueuer.OnEnqueued += value; }
            remove { enqueuer.OnEnqueued -= value; }
        }

        public event Action<T, object> OnInterrupted
        {
            add { enqueuer.OnInterrupted += value; }
            remove { enqueuer.OnInterrupted -= value; }
        }

        public bool IsRunning
        {
            get { return enqueuer.IsRunning; }
        }

        [SerializeField]
        private UnityAICommandExecutor<T> executor;

        private IAICommandEnqueuer<T> enqueuer;

        private void Awake()
        {
            enqueuer = new AICommandEnqueuer<T>(executor);
        }

        public void Enqueue(T key, object args)
        {
            enqueuer.Enqueue(key, args);
        }

        public void Interrupt()
        {
            enqueuer.Interrupt();
        }

        public IEnumerable<(T, object)> GetQueue()
        {
            return enqueuer.GetQueue();
        }
    }
}