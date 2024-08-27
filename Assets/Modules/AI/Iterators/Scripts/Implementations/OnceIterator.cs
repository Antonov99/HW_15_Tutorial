namespace AI.Iterators
{
    public sealed class OnceIterator<T> : Iterator<T>
    {
        public OnceIterator(T[] moveItems) : base(moveItems)
        {
        }

        public override bool MoveNext()
        {
            if (pointer + 1 < movePoints.Length)
            {
                pointer++;
                return true;
            }

            return false;
        }

        public override void Reset()
        {
            pointer = 0;
        }

        public override void Dispose()
        {
        }
    }
}