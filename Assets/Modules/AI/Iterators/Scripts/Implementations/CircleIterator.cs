namespace AI.Iterators
{
    public sealed class CircleIterator<T> : Iterator<T>
    {
        public CircleIterator(T[] items) : base(items)
        {
        }

        public override bool MoveNext()
        {
            pointer = (pointer + 1) % movePoints.Length;
            return true;
        }

        public override void Reset()
        {
        }

        public override void Dispose()
        {
        }
    }
}