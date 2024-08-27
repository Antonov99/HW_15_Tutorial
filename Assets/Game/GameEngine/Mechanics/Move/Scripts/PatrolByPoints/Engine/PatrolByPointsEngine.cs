using System;
using System.Collections.Generic;
using Elementary;
using Sirenix.OdinInspector;

namespace Game.GameEngine.Mechanics
{
    public sealed class PatrolByPointsEngine : IPatrolByPointsEngine
    {
        public event Action<PatrolByPointsOperation> OnPatrolStarted;

        public event Action<PatrolByPointsOperation> OnPatrolStopped;

        [ShowInInspector, ReadOnly, PropertyOrder(-10), PropertySpace]
        public bool IsPatrol { get; private set; }

        [ShowInInspector, ReadOnly, PropertyOrder(-9)]
        public PatrolByPointsOperation CurrentOperation { get; private set; }

        [ShowInInspector, ReadOnly, PropertyOrder(-8), PropertySpace]
        private List<ICondition<PatrolByPointsOperation>> preconditions = new();

        [ShowInInspector, ReadOnly, PropertyOrder(-7)]
        private List<IAction<PatrolByPointsOperation>> startActions = new();

        [ShowInInspector, ReadOnly, PropertyOrder(-6)]
        private List<IAction<PatrolByPointsOperation>> stopActions = new();

        [Title("Methods")]
        [Button]
        public bool CanStartPatrol(PatrolByPointsOperation operation)
        {
            if (IsPatrol)
            {
                return false;
            }

            for (int i = 0, count = preconditions.Count; i < count; i++)
            {
                var condition = preconditions[i];
                if (!condition.IsTrue(operation))
                {
                    return false;
                }
            }

            return true;
        }

        [Button]
        public void StartPatrol(PatrolByPointsOperation operation)
        {
            if (!CanStartPatrol(operation))
            {
                return;
            }

            for (int i = 0, count = startActions.Count; i < count; i++)
            {
                var action = startActions[i];
                action.Do(operation);
            }

            CurrentOperation = operation;
            IsPatrol = true;
            OnPatrolStarted?.Invoke(operation);
        }

        [Button]
        public void StopPatrol()
        {
            if (!IsPatrol)
            {
                return;
            }

            var operation = CurrentOperation;
            for (int i = 0, count = stopActions.Count; i < count; i++)
            {
                var action = stopActions[i];
                action.Do(operation);
            }

            IsPatrol = false;
            CurrentOperation = default;
            OnPatrolStopped?.Invoke(operation);
        }

        public void AddPreconditions(params ICondition<PatrolByPointsOperation>[] conditions)
        {
            preconditions.AddRange(conditions);
        }

        public void AddPreconditions(IEnumerable<ICondition<PatrolByPointsOperation>> conditions)
        {
            preconditions.AddRange(conditions);
        }

        public void AddPreconditon(ICondition<PatrolByPointsOperation> condition)
        {
            preconditions.Add(condition);
        }

        public void AddStartActions(IEnumerable<IAction<PatrolByPointsOperation>> actions)
        {
            startActions.AddRange(actions);
        }

        public void AddStartAction(IAction<PatrolByPointsOperation> action)
        {
            startActions.Add(action);
        }

        public void AddStopActions(IEnumerable<IAction<PatrolByPointsOperation>> actions)
        {
            stopActions.AddRange(actions);
        }

        public void AddStopAction(IAction<PatrolByPointsOperation> action)
        {
            stopActions.Add(action);
        }
    }
}