using UnityEngine;

namespace AI.Tasks
{
    public abstract class AITask : IAITask
    {
        public bool IsPlaying
        {
            get { return isPlaying; }
        }

        private bool isPlaying;

        private IAITaskCallback callback;

        public void Do(IAITaskCallback callback)
        {
            if (isPlaying)
            {
                Debug.LogWarning($"Task {GetType().Name} is already started!");
                return;
            }

            isPlaying = true;
            this.callback = callback;
            Do();
        }

        public void Cancel()
        {
            if (!isPlaying)
            {
                return;
            }

            isPlaying = false;
            callback = null;
            OnCancel();
        }
        
        protected abstract void Do();

        protected void Return(bool success)
        {
            isPlaying = false;
            
            var callback = this.callback;
            this.callback = null;

            callback?.Invoke(this, success);
        }

        protected virtual void OnCancel()
        {
        }
    }
}