using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    [AddComponentMenu("Entities/Entity")]
    public class MonoEntityStd : MonoEntity, ISerializationCallbackReceiver
    {
        [Space]
        [SerializeField]
        private List<MonoBehaviour> monoElements = new();

        [Space]
        [SerializeField]
        private List<ScriptableObject> scriptableElements = new();

        [Space]
        [SerializeReference]
        private List<object> referenceElements = new();

        private ListEntity entity;

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

        public virtual void OnAfterDeserialize()
        {
            var allElements = new List<object>();
            allElements.AddRange(monoElements);
            allElements.AddRange(scriptableElements);
            allElements.AddRange(referenceElements);
            entity = new ListEntity(allElements);
        }

        public virtual void OnBeforeSerialize()
        {
        }
    }
}