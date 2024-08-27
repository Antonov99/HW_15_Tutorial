using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Iterators
{
    public abstract class Iterator<T> : IEnumerator<T>
    {
        public T Current
        {
            get { return movePoints[pointer]; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        protected T[] movePoints;

        protected int pointer;

        public Iterator(T[] moveItems)
        {
            SetPoints(moveItems);
        }

        public Iterator()
        {
            movePoints = new T[0];
        }

        public void SetPoints(T[] movePoints)
        {
            this.movePoints = movePoints;
            pointer = 0;
        }

        public abstract bool MoveNext();

        public abstract void Reset();

        public abstract void Dispose();
    }
}