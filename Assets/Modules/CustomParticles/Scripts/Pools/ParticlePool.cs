using System.Collections.Generic;
using UnityEngine;

namespace CustomParticles
{
    public abstract class ParticlePool<T> : MonoBehaviour where T : Object
    {
        [SerializeField]
        private Transform pool;
        
        [Space]
        [SerializeField]
        private T particlePrefab;

        [SerializeField]
        private int initialSize = 16;

        private List<T> particlePool;

        protected virtual void Awake()
        {
            particlePool = new List<T>(initialSize);
            for (var i = 0; i < initialSize; i++)
            {
                var particle = Instantiate(particlePrefab, pool);
                particlePool.Add(particle);
            }

            pool.gameObject.SetActive(false);
        }

        public virtual T Get(Transform parent)
        {
            var count = particlePool.Count;
            if (count <= 0)
            {
                return Instantiate(particlePrefab, parent);
            }

            var lastIndex = count - 1;
            var particle = particlePool[lastIndex];
            particlePool.RemoveAt(lastIndex);

            var transform = GetTransform(particle);
            transform.SetParent(parent);
            return particle;
        }

        public virtual void Release(T particle)
        {
            var transform = GetTransform(particle);
            transform.SetParent(pool);
            particlePool.Add(particle);
        }

        protected abstract Transform GetTransform(T particle);
    }
}