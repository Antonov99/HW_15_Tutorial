using System;
using System.Collections.Generic;

namespace AI.Agents
{
    public abstract class Agent_MoveByPoints<T> : AgentCoroutine
    {
        public event Action OnPathFinished;

        public bool IsPathFinished
        {
            get { return isPathFinished; }
        }

        private readonly List<T> currentPath = new ();

        private int pointer;

        private bool isPathFinished;
        
        public void SetPath(IEnumerable<T> points)
        {
            currentPath.Clear();
            currentPath.AddRange(points);

            pointer = 0;
            isPathFinished = false;
        }

        protected override void Update()
        {
            if (isPathFinished)
            {
                return;
            }

            if (pointer >= currentPath.Count)
            {
                isPathFinished = true;
                OnPathFinished?.Invoke();
                return;
            }

            var targetPoint = currentPath[pointer];
            if (CheckPointReached(targetPoint))
            {
                pointer++;
            }
            else
            {
                MoveToPoint(targetPoint);
            }
        }

        protected abstract bool CheckPointReached(T point);

        protected abstract void MoveToPoint(T target);
    }
}