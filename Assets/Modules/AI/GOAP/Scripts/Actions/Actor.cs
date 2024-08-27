using Sirenix.OdinInspector;
using UnityEngine;

namespace AI.GOAP
{
    public abstract class Actor : MonoBehaviour, IActor
    {
        public IFactState ResultState
        {
            get { return resultState; }
        }

        public IFactState RequiredState
        {
            get { return requiredState; }
        }

        [ShowInInspector, ReadOnly, PropertyOrder(-10)]
        public bool IsPlaying
        {
            get { return isPlaying; }
        }

        [Space]
        [SerializeField]
        protected FactState resultState;

        [SerializeField]
        protected FactState requiredState;

        private bool isPlaying;

        private IActor.Callback callback;

        public abstract int EvaluateCost();

        public abstract bool IsValid();

        public void Play(IActor.Callback callback)
        {
            if (isPlaying)
            {
                return;
            }

            this.callback = callback;
            isPlaying = true;
            Play();
        }

        public void Cancel()
        {
            if (!isPlaying)
            {
                return;
            }

            OnCancel();
            isPlaying = false;
            callback = null;
            OnDispose();
        }

        protected abstract void Play();

        protected virtual void Return(bool success)
        {
            if (!isPlaying)
            {
                return;
            }
            
            isPlaying = false;
            OnReturn();
            OnDispose();
            InvokeCallback(success);
        }

        #region Callbacks

        protected virtual void OnReturn()
        {
        }

        protected virtual void OnCancel()
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