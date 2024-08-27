using System;
using System.Collections.Generic;
using Elementary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    public sealed class MoveToPositionMotor : IMoveToPositionMotor
    {
        public event Action<Vector3> OnMoveStarted;

        public event Action<Vector3> OnMoveStopped;

        [ShowInInspector, ReadOnly, PropertyOrder(-10), PropertySpace]
        public bool IsMove { get; private set; }

        [ShowInInspector, ReadOnly, PropertyOrder(-9)]
        public Vector3 TargetPosition { get; private set; }

        [ShowInInspector, ReadOnly, PropertyOrder(-8), PropertySpace]
        private List<ICondition<Vector3>> preconditions = new();

        [ShowInInspector, ReadOnly, PropertyOrder(-7)]
        private List<IAction<Vector3>> startActions = new();

        [ShowInInspector, ReadOnly, PropertyOrder(-6)]
        private List<IAction<Vector3>> stopActions = new();

        [Title("Methods")]
        [Button]
        public bool CanStartMove(Vector3 operation)
        {
            if (IsMove)
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
        public void StartMove(Vector3 operation)
        {
            if (!CanStartMove(operation))
            {
                return;
            }

            for (int i = 0, count = startActions.Count; i < count; i++)
            {
                var action = startActions[i];
                action.Do(operation);
            }

            TargetPosition = operation;
            IsMove = true;
            OnMoveStarted?.Invoke(operation);
        }

        [Button]
        public void StopMove()
        {
            if (!IsMove)
            {
                return;
            }

            var operation = TargetPosition;
            for (int i = 0, count = stopActions.Count; i < count; i++)
            {
                var action = stopActions[i];
                action.Do(operation);
            }

            IsMove = false;
            TargetPosition = default;
            OnMoveStopped?.Invoke(operation);
        }

        public void AddPreconditions(params ICondition<Vector3>[] conditions)
        {
            preconditions.AddRange(conditions);
        }

        public void AddPreconditions(IEnumerable<ICondition<Vector3>> conditions)
        {
            preconditions.AddRange(conditions);
        }

        public void AddPreconditon(ICondition<Vector3> condition)
        {
            preconditions.Add(condition);
        }

        public void AddStartActions(IEnumerable<IAction<Vector3>> actions)
        {
            startActions.AddRange(actions);
        }

        public void AddStartAction(IAction<Vector3> action)
        {
            startActions.Add(action);
        }

        public void AddStopActions(IEnumerable<IAction<Vector3>> actions)
        {
            stopActions.AddRange(actions);
        }

        public void AddStopAction(IAction<Vector3> action)
        {
            stopActions.Add(action);
        }
    }
}