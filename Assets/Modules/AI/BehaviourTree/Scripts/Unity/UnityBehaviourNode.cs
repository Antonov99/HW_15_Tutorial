using Sirenix.OdinInspector;
using UnityEngine;

namespace AI.BTree
{
    public abstract class UnityBehaviourNode : MonoBehaviour, IBehaviourNode
    {
        [ShowInInspector, ReadOnly]
        public bool IsRunning { get; private set; }

        private IBehaviourCallback callback;

        [Button]
        public void Run(IBehaviourCallback callback)
        {
            if (IsRunning)
            {
                return;
            }

            this.callback = callback;
            IsRunning = true;
            Run();
        }

        [Button]
        public void Abort()
        {
            if (!IsRunning)
            {
                return;
            }

            OnAbort();
            IsRunning = false;
            callback = null;
            OnDispose();
        }

        protected abstract void Run();

        protected void Return(bool success)
        {
            if (!IsRunning)
            {
                return;
            }

            IsRunning = false;
            OnReturn(success);
            OnDispose();
            InvokeCallback(success);
        }

        #region Callbacks

        protected virtual void OnReturn(bool success)
        {
        }

        protected virtual void OnAbort()
        {
        }

        protected virtual void OnDispose()
        {
        }

        #endregion

        private void InvokeCallback(bool success)
        {
            if (this.callback == null)
            {
                return;
            }

            var callback = this.callback;
            this.callback = null;
            callback.Invoke(this, success);
        }
    }
}