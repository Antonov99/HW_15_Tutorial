using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Commands
{
    public class AICommandExecutor<T> : IAICommandExecutor<T>, IAICommandCallback
    {
        public event Action<T, object> OnStarted;

        public event Action<T, object> OnFinished;

        public event Action<T, object> OnInterrupted;

        public bool IsRunning
        {
            get { return currentCommand != null; }
        }

        private readonly Dictionary<T, IAICommand> commands = new();

        private IAICommand currentCommand;

        private T currentKey;

        private object currentArgs;

        public AICommandExecutor(Dictionary<T, IAICommand> commands)
        {
            this.commands = commands;
        }

        public AICommandExecutor()
        {
        }

        public void Execute(T key, object args = null)
        {
            if (IsRunning)
            {
                Debug.LogWarning($"Other command {currentCommand.GetType().Name} is already run!");
                return;
            }

            if (!commands.TryGetValue(key, out currentCommand))
            {
                Debug.LogWarning($"Command with {key} is not found!");
                return;
            }

            currentKey = key;
            currentArgs = args;

            OnStarted?.Invoke(key, args);
            currentCommand.Execute(args, callback: this);
        }

        public void Interrupt()
        {
            if (!IsRunning)
            {
                return;
            }

            currentCommand.Interrupt();
            currentCommand = null;
            OnInterrupted?.Invoke(currentKey, currentArgs);
        }

        public bool TryGetRunningInfo(out T key, out object args)
        {
            key = currentKey;
            args = currentArgs;
            return currentCommand != null;
        }

        public void RegisterCommand(T key, IAICommand command)
        {
            commands.Add(key, command);
        }

        public void UnregisterCommand(T key)
        {
            commands.Remove(key);
        }

        void IAICommandCallback.Invoke(IAICommand command, object args, bool success)
        {
            currentCommand = null;
            OnFinished?.Invoke(currentKey, currentArgs);
        }
    }
}