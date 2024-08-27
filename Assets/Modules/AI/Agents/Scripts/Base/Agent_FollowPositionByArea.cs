using System;
using System.Collections;
using UnityEngine;

namespace AI.Agents
{
    public abstract class Agent_FollowPositionByArea : Agent
    {
        public event Action<bool> OnTargetReached;

        protected abstract Agent_MoveToTarget<Vector3> MoveAgent { get; }
        
        private bool isTargetReached;

        private Coroutine calculatePathCoroutine;

        private Coroutine checkTargetReachedCoroutine;

        private YieldInstruction calculatePathPeriod;

        private YieldInstruction checkTargetReachedPeriod;
        
        private float sqrStoppingDistance;

        public void SetCheckTargetReachedPeriod(YieldInstruction period)
        {
            checkTargetReachedPeriod = period;
        }

        public void SetCalculatePathPeriod(YieldInstruction period)
        {
            calculatePathPeriod = period;
        }
        
        public void SetStoppingDistance(float stoppingDistance)
        {
            sqrStoppingDistance = Mathf.Pow(stoppingDistance, 2);
        }

        protected override void OnStart()
        {
            isTargetReached = false;

            var context = MonoHelper.Instance;
            calculatePathCoroutine = context.StartCoroutine(CalculatePathRoutine());
            checkTargetReachedCoroutine = context.StartCoroutine(CheckTargetReachedRoutine());
            MoveAgent.Play();
        }

        protected override void OnStop()
        {
            MoveAgent.Stop();

            var context = MonoHelper.Instance;
            if (calculatePathCoroutine != null)
            {
                context.StopCoroutine(calculatePathCoroutine);
                calculatePathCoroutine = null;
            }

            if (checkTargetReachedCoroutine != null)
            {
                context.StopCoroutine(checkTargetReachedCoroutine);
                checkTargetReachedCoroutine = null;
            }
        }

        private IEnumerator CheckTargetReachedRoutine()
        {
            while (true)
            {
                yield return checkTargetReachedPeriod;
                UpdateTargetReach();
            }
        }
        
        private IEnumerator CalculatePathRoutine()
        {
            while (true)
            {
                yield return calculatePathPeriod;
                CalculateNextPoint();
            }
        }

        private void UpdateTargetReach()
        {
            var isTargetReached = CheckTargetReached();

            if (isTargetReached && !this.isTargetReached)
            {
                this.isTargetReached = true;
                OnTargetReached?.Invoke(true);
            }

            if (!isTargetReached && this.isTargetReached)
            {
                this.isTargetReached = false;
                OnTargetReached?.Invoke(false);
            }
        }

        private void CalculateNextPoint()
        {
            if (FindNextPosition(out var nextPosition))
            {
                MoveAgent.SetTarget(nextPosition);
            }
        }

        private bool CheckTargetReached()
        {
            var distanceVector = EvaluateTargetPosition() - EvaluateCurrentPosition();
            return distanceVector.sqrMagnitude <= sqrStoppingDistance;
        }

        protected abstract bool FindNextPosition(out Vector3 availablePosition);

        protected abstract Vector3 EvaluateCurrentPosition();

        protected abstract Vector3 EvaluateTargetPosition();
    }
}