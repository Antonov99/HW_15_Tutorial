using System;
using System.Collections.Generic;
using Elementary;
using Declarative;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [Serializable]
    public sealed class MoveInDirectionMotor : IMoveInDirectionMotor
    {
        private static readonly Vector3 ZERO_DIRECTION = Vector3.zero;

        public event Action OnStartMove;

        public event Action OnStopMove;

        public bool IsMoving
        {
            get { return moveRequired && direction != ZERO_DIRECTION; }
        }

        public Vector3 Direction
        {
            get { return direction; }
        }

        [ShowInInspector, ReadOnly, PropertyOrder(-11)]
        private bool isEnabled;

        [ShowInInspector, ReadOnly, PropertyOrder(-10)]
        private bool moveRequired;

        [ShowInInspector, ReadOnly, PropertyOrder(-9)]
        private bool finishMove;

        [ShowInInspector, ReadOnly, PropertyOrder(-8)]
        private Vector3 direction;

        [ShowInInspector, ReadOnly, PropertyOrder(-6)]
        private List<ICondition<Vector3>> preconditions = new();

        [ShowInInspector, ReadOnly, PropertyOrder(-5)]
        private List<IAction<Vector3>> startActions = new();

        [ShowInInspector, ReadOnly, PropertyOrder(-4)]
        private List<IAction<Vector3>> stopActions = new();

        public bool CanMove(Vector3 direction)
        {
            for (int i = 0, count = preconditions.Count; i < count; i++)
            {
                var condition = preconditions[i];
                if (!condition.IsTrue(direction))
                {
                    return false;
                }
            }

            return true;
        }

        public void RequestMove(Vector3 direction)
        {
            if (!CanMove(direction))
            {
                return;
            }

            this.direction = direction;
            finishMove = false;

            if (!moveRequired)
            {
                moveRequired = true;
                StartMove();
            }
        }

        public void Update()
        {
            if (!moveRequired)
            {
                return;
            }

            if (finishMove)
            {
                moveRequired = false;
                StopMove();
            }

            finishMove = true;
        }

        public void Interrupt()
        {
            if (!moveRequired)
            {
                return;
            }

            finishMove = false;
            moveRequired = false;
            StopMove();
        }
        
        public void AddPrecondition(Func<Vector3, bool> condition)
        {
            preconditions.Add(new ConditionDelegate<Vector3>(condition));
        }

        public void AddPrecondition(ICondition<Vector3> condition)
        {
            preconditions.Add(condition);
        }

        public void AddPreconditions(params ICondition<Vector3>[] conditions)
        {
            preconditions.AddRange(conditions);
        }

        public void AddPreconditions(IEnumerable<ICondition<Vector3>> conditions)
        {
            preconditions.AddRange(conditions);
        }

        public void RemovePrecondition(ICondition<Vector3> condition)
        {
            preconditions.Remove(condition);
        }

        public void AddStartAction(Action<Vector3> action)
        {
            startActions.Add(new ActionDelegate<Vector3>(action));
        }

        public void AddStartAction(IAction<Vector3> action)
        {
            startActions.Add(action);
        }

        public void AddStartActions(IEnumerable<IAction<Vector3>> actions)
        {
            startActions.AddRange(actions);
        }

        public void RemoveStartAction(IAction<Vector3> action)
        {
            startActions.Remove(action);
        }

        public void AddStopAction(IAction<Vector3> action)
        {
            stopActions.Add(action);
        }

        public void AddStopActions(IEnumerable<IAction<Vector3>> actions)
        {
            stopActions.AddRange(actions);
        }

        public void RemoveStopAction(IAction<Vector3> action)
        {
            stopActions.Remove(action);
        }

        private void StartMove()
        {
            for (int i = 0, count = startActions.Count; i < count; i++)
            {
                var action = startActions[i];
                action.Do(direction);
            }

            OnStartMove?.Invoke();
        }

        private void StopMove()
        {
            for (int i = 0, count = stopActions.Count; i < count; i++)
            {
                var action = stopActions[i];
                action.Do(direction);
            }

            OnStopMove?.Invoke();
        }
    }
}