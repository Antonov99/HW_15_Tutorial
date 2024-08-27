using System;

namespace Elementary
{
    public sealed class ActionDelegate : IAction
    {
        private readonly Action action;

        public ActionDelegate(Action action)
        {
            this.action = action;
        }

        public void Do()
        {
            action?.Invoke();
        }
    }

    public sealed class ActionDelegate<T> : IAction<T>
    {
        private readonly Action<T> action;

        public ActionDelegate(Action<T> action)
        {
            this.action = action;
        }

        public void Do(T args)
        {
            action?.Invoke(args);
        }
    }
}