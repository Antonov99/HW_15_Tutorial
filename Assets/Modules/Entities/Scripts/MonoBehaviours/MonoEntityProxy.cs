using UnityEngine;

namespace Entities
{
    [AddComponentMenu("Entities/Entity Proxy")]
    public sealed class MonoEntityProxy : MonoEntity
    {
        [SerializeField]
        public MonoEntity entity;

        public override T Get<T>()
        {
            return entity.Get<T>();
        }

        public override object[] GetAll()
        {
            return entity.GetAll();
        }

        public override bool TryGet<T>(out T element)
        {
            return entity.TryGet(out element);
        }
    }
}