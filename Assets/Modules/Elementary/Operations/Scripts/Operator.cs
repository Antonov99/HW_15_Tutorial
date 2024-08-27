using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Elementary
{
    [Serializable]
    public sealed class Operator<T> : IOperator<T>
    {
        public event Action<T> OnStarted;

        public event Action<T> OnStopped;

        private ActionComposite<T> onStarted;

        private ActionComposite<T> onStopped;

        [ShowInInspector, ReadOnly]
        public bool IsActive
        {
            get { return Current != null; }
        }

        [ShowInInspector, ReadOnly]
        public T Current { get; private set; }

        private ConditionComposite<T> conditions;

        private ActionComposite<T> startActions;

        private ActionComposite<T> stopActions;

        [Title("Methods")]
        [Button]
        public bool CanStart(T operation)
        {
            if (IsActive)
            {
                return false;
            }

            if (conditions == null)
            {
                return true;
            }

            return conditions.IsTrue(operation);
        }

        [Button]
        public void DoStart(T operation)
        {
            if (!CanStart(operation))
            {
                return;
            }

            Current = operation;
            startActions?.Do(operation);
            OnStarted?.Invoke(operation);
        }

        [Button]
        public void Stop()
        {
            if (!IsActive)
            {
                return;
            }

            var operation = Current;

            Current = default;
            stopActions?.Do(operation);
            OnStopped?.Invoke(operation);
        }

        public void AddConditions(params ICondition<T>[] conditions)
        {
            this.conditions += conditions;
        }

        public void AddConditions(IEnumerable<ICondition<T>> conditions)
        {
            this.conditions += conditions;
        }

        public ICondition<T> AddCondition(Func<T, bool> condition)
        {
            var conditionDelegate = new ConditionDelegate<T>(condition);
            conditions += conditionDelegate;
            return conditionDelegate;
        }

        public void AddCondition(ICondition<T> condition)
        {
            conditions += condition;
        }

        public void RemoveCondition(ICondition<T> condition)
        {
            conditions -= condition;
        }

        public void AddStartActions(params IAction<T>[] actions)
        {
            startActions += actions;
        }

        public void AddStartActions(IEnumerable<IAction<T>> actions)
        {
            startActions += actions;
        }

        public ActionDelegate<T> AddStartAction(Action<T> action)
        {
            var actionDelegate = new ActionDelegate<T>(action);
            startActions += actionDelegate;
            return actionDelegate;
        }

        public void AddStartAction(IAction<T> action)
        {
            startActions += action;
        }
        
        public void RemoveStartAction(IAction<T> action)
        {
            startActions -= action;
        }

        public void AddStopActions(IEnumerable<IAction<T>> actions)
        {
            stopActions += actions;
        }

        public void AddStopAction(IAction<T> action)
        {
            stopActions += action;
        }
        
        public ActionDelegate<T> AddStopAction(Action<T> action)
        {
            var actionDelegate = new ActionDelegate<T>(action);
            stopActions += actionDelegate;
            return actionDelegate;
        }
        
        public void RemoveStopAction(IAction<T> action)
        {
            stopActions -= action;
        }
    }
}