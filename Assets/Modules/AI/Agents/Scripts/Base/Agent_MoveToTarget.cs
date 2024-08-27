using System;

namespace AI.Agents
{
    public abstract class Agent_MoveToTarget<T> : AgentCoroutine
    {
        public event Action<bool> OnTargetReached;

        public bool IsTargetReached
        {
            get { return isTargetReached; }
        }

        private T target;

        private bool isTargetReached;

        public void SetTarget(T target)
        {
            this.target = target;
        }

        protected override void OnStart()
        {
            base.OnStart();
            isTargetReached = false;
        }

        protected override void Update()
        {
            var isTargetReached = CheckTargetReached(target);

            if (isTargetReached && !this.isTargetReached)
            {
                this.isTargetReached = true;
                OnTargetReached?.Invoke(true);
            }
            else if (!isTargetReached && this.isTargetReached)
            {
                this.isTargetReached = false;
                OnTargetReached?.Invoke(false);
            }

            if (!isTargetReached)
            {
                MoveToTarget(target);
            }
        }

        protected abstract bool CheckTargetReached(T target);

        protected abstract void MoveToTarget(T target);
    }
}