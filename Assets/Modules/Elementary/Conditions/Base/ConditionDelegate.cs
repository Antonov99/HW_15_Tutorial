using System;

namespace Elementary
{
    public sealed class ConditionDelegate : ICondition
    {
        private readonly Func<bool> condition;

        public ConditionDelegate(Func<bool> condition)
        {
            this.condition = condition;
        }

        public bool IsTrue()
        {
            return condition.Invoke();
        }
    }

    public sealed class ConditionDelegate<T> : ICondition<T>
    {
        private readonly Func<T, bool> condition;

        public ConditionDelegate(Func<T, bool> condition)
        {
            this.condition = condition;
        }

        public bool IsTrue(T args)
        {
            return condition.Invoke(args);
        }
    }
}