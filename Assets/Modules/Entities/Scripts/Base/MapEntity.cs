using System;
using System.Collections.Generic;
using System.Linq;

namespace Entities
{
    public class MapEntity : IEntity
    {
        private readonly Dictionary<Type, object> elements;

        public MapEntity()
        {
            elements = new Dictionary<Type, object>();
        }

        public MapEntity(Dictionary<Type, object> elements)
        {
            this.elements = new Dictionary<Type, object>(elements);
        }

        public T Get<T>()
        {
            if (elements.TryGetValue(typeof(T), out var element))
            {
                return (T) element;
            }

            throw new EntityException($"Element of type {typeof(T).Name} is not found!");
        }

        public object[] GetAll()
        {
            return elements.Values.ToArray();
        }

        public void Add<T>(T element)
        {
            if (elements.ContainsKey(typeof(T)))
            {
                throw new EntityException($"Element of type {typeof(T).Name} is already exists!");
            }
            
            elements.Add(typeof(T), element);
        }

        public void Remove<T>(T element)
        {
            elements.Remove(typeof(T));
        }

        public bool TryGet<T>(out T result)
        {
            if (elements.TryGetValue(typeof(T), out var element))
            {
                result = (T) element;
                return true;
            }

            result = default;
            return false;
        }
    }
}