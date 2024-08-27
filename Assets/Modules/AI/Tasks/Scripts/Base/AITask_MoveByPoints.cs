using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Tasks
{
    public abstract class AITask_MoveByPoints<T> : AITaskCoroutine
    {
        private readonly List<T> currentPath = new();

        private YieldInstruction framePeriod;

        public void SetFramePeriod(YieldInstruction framePeriod)
        {
            this.framePeriod = framePeriod;
        }

        public void SetPath(IEnumerable<T> points)
        {
            currentPath.Clear();
            currentPath.AddRange(points);
        }

        protected override IEnumerator DoAsync()
        {
            for (var i = 0; i < currentPath.Count; i++)
            {
                var nextPoint = currentPath[i];
                yield return MoveToNextPoint(nextPoint);
            }

            yield return framePeriod;
            Return(true);
        }

        private IEnumerator MoveToNextPoint(T target)
        {
            while (!CheckPointReached(target))
            {
                MoveToPoint(target);
                yield return framePeriod;
            }
        }

        protected abstract bool CheckPointReached(T target);

        protected abstract void MoveToPoint(T target);
    }
}