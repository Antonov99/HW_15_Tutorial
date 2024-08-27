using System;
using AI.Commands;
using UnityEngine;

namespace Game.GameEngine.AI
{
    public sealed class UComponent_ExecuteCommand : MonoBehaviour, IComponent_ExecuteCommand
    {
        public bool IsRunning
        {
            get { return executor; }
        }

        [SerializeField]
        private UnityAICommandExecutor<Type> executor;
        
        public void Execute<T>(T args)
        {
            executor.Execute(args);
        }

        public void ExecuteForce<T>(T args)
        {
            executor.ExecuteForce(args);
        }

        public void Interrupt()
        {
            executor.Interrupt();
        }
    }
}