using System;
using System.Collections.Generic;
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace AI.Commands
{
    public class AICommandEnqueuer<T> : IAICommandEnqueuer<T>
    {
        public event Action<T, object> OnEnqueued;

        public event Action<T, object> OnInterrupted;

        public bool IsRunning
        {
            get { return commandQueue.Count > 0 || executor is {IsRunning: true}; }
        }

        private IAICommandExecutor<T> executor;

        private readonly Queue<(T, object)> commandQueue = new();

        public AICommandEnqueuer(IAICommandExecutor<T> executor)
        {
            this.executor = executor;
        }

        public void Construct(IAICommandExecutor<T> executor)
        {
            this.executor = executor;
        }

        public void Start()
        {
            executor.OnFinished += OnCommandFinished;
        }

        public void Stop()
        {
            executor.OnFinished -= OnCommandFinished;
        }
        
        public void Enqueue(T key, object args)
        {
            if (executor.IsRunning)
            {
                commandQueue.Enqueue(new (key, args));
            }
            else
            {
                executor.Execute(key, args);
            }
        }

        public void Interrupt()
        {
            executor.Interrupt();
            commandQueue.Clear();
        }

        public IEnumerable<(T, object)> GetQueue()
        {
            return commandQueue;
        }
        
        private void OnCommandFinished(T prevKey, object prevArgs)
        {
            if (commandQueue.Count > 0)
            {
                var (key, args) = commandQueue.Dequeue();
                executor.Execute(key, args);
            }
        }
    }
}