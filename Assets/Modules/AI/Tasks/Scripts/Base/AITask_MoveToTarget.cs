using System.Collections;
using UnityEngine;

namespace AI.Tasks
{
    public abstract class AITask_MoveToTarget<T> : AITaskCoroutine
    {
        private YieldInstruction framePeriod;

        private T target;

        public void SetFramePeriod(YieldInstruction framePeriod)
        {
            this.framePeriod = framePeriod;
        }
        
        public void SetTarget(T target)
        {
            this.target = target;
        }

        protected override IEnumerator DoAsync()
        {
            while (!CheckTargetReached(target))
            {
                MoveToTarget(target);
                yield return framePeriod;
            }

            yield return framePeriod; //A little bit clumsy... Wait next frame
            Return(true);
        }

        protected abstract bool CheckTargetReached(T target);

        protected abstract void MoveToTarget(T target);
    }
}