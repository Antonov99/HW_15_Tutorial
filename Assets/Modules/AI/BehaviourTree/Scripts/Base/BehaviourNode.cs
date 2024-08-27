#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace AI.BTree
{
    public abstract class BehaviourNode : IBehaviourNode
    {
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly, PropertyOrder(-1000)]
#endif
        public bool IsRunning
        {
            get { return isRunning; }
        }

        private bool isRunning;

        private IBehaviourCallback callback;

        public void Run(IBehaviourCallback callback)
        {
            if (isRunning)
            {
                return;
            }

            this.callback = callback;
            isRunning = true;
            Run();
        }

        public void Abort()
        {
            if (!isRunning)
            {
                return;
            }

            OnAbort();
            isRunning = false;
            callback = null;
            OnDispose();
        }

        protected abstract void Run();

        protected void Return(bool success)
        {
            if (!isRunning)
            {
                return;
            }

            isRunning = false;
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