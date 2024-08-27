using System;

namespace Elementary
{
    public sealed class FuncValue<T> : IValue<T>
    {
        public T Current
        {
            get { return function.Invoke(); }
        }

        private readonly Func<T> function;

        public FuncValue(Func<T> function)
        {
            this.function = function;
        }
    }
}