using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public abstract class MonoEntityBase : MonoEntity
    {
        private readonly ListEntity entity = new();

        public override T Get<T>()
        {
            try
            {
                return entity.Get<T>();
            }
            catch (EntityException exception)
            {
                Debug.LogError(exception.Message, this);
                throw;
            }
        }

        public override object[] GetAll()
        {
            return entity.GetAll();
        }

        public T[] GetAll<T>()
        {
            return entity.GetAll<T>();
        }

        public void Add(object element)
        {
            entity.Add(element);
        }

        public void Remove(object element)
        {
            entity.Remove(element);
        }

        public void AddRange(params object[] elements)
        {
            entity.AddRange(elements);
        }

        public void AddRange(IEnumerable<object> elements)
        {
            entity.AddRange(elements);
        }

        public override bool TryGet<T>(out T element)
        {
            return entity.TryGet(out element);
        }
    }
}