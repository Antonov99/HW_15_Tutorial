using UnityEngine;

namespace AI.Commands
{
    public abstract class UnityAICommand : MonoBehaviour, IAICommand
    {
        public bool IsPlaying
        {
            get { return isPlaying; }
        }

        private bool isPlaying;

        private IAICommandCallback callback;

        private object args;

        public void Execute(object args, IAICommandCallback callback)
        {
            if (isPlaying)
            {
                Debug.LogWarning($"Command {GetType().Name} is already started!");
                return;
            }

            this.args = args;
            this.callback = callback;
            isPlaying = true;
            Execute(args);
        }

        public void Interrupt()
        {
            if (!isPlaying)
            {
                return;
            }

            OnInterrupt();
            isPlaying = false;
            args = null;
            callback = null;
        }

        protected abstract void Execute(object args);

        protected void Return(bool success)
        {
            isPlaying = false;

            var callback = this.callback;
            this.callback = null;

            var args = this.args;
            this.args = null;

            callback?.Invoke(this, args, success);
        }

        protected virtual void OnInterrupt()
        {
        }
    }

    public abstract class UnityAICommand<T> : UnityAICommand
    {
        protected sealed override void Execute(object args)
        {
            if (args is not T tArgs)
            {
                Debug.LogWarning("Mismatch command type");
                Return(false);
                return;
            }

            Execute(tArgs);
        }

        protected abstract void Execute(T args);
    }
}