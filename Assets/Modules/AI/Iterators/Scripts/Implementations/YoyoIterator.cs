namespace AI.Iterators
{
    public sealed class YoyoIterator<T> : Iterator<T>
    {
        public bool Forward { get; set; }

        public YoyoIterator(T[] items) : base(items)
        {
        }

        public override bool MoveNext()
        {
            if (Forward)
            {
                if (pointer + 1 < movePoints.Length)
                {
                    pointer++;
                }
                else
                {
                    Forward = false;
                    pointer--;
                }
            }
            else
            {
                if (pointer - 1 >= 0)
                {
                    pointer--;
                }
                else
                {
                    Forward = true;
                    pointer++;
                }
            }

            return true;
        }

        public override void Reset()
        {
            pointer = 0;
            Forward = true;
        }

        public override void Dispose()
        {
        }
    }
}