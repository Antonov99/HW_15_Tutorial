using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Elementary
{
    public abstract class ColliderDetection : MonoBehaviour
    {
        public event Action OnCollidersUpdated;

        [Space]
        [SerializeField]
        [FormerlySerializedAs("playOnAwake")]
        private bool playOnStart;
        
        [Space]
        [SerializeField]
        private float minScanPeriod = 0.1f;

        [SerializeField]
        private float maxScanPeriod = 0.2f;

        [Space]
        [SerializeField]
        private int bufferCapacity = 64;

        [Title("Debug")]
        [PropertyOrder(10)]
        [ReadOnly]
        [ShowInInspector]
        public bool IsPlaying
        {
            get { return coroutine != null; }
        }

        [PropertyOrder(11)]
        [ReadOnly]
        [ShowInInspector]
        private Collider[] buffer;

        private int bufferSize;

        private Coroutine coroutine;

        private readonly List<IColliderDetectionHandler> listeners = new();
        
        private void Start()
        {
            buffer = new Collider[bufferCapacity];
            if (playOnStart)
            {
                Play();
            }
        }

        public void GetCollidersNonAlloc(Collider[] buffer, out int size)
        {
            size = bufferSize;
            Array.Copy(this.buffer, buffer, size);
        }

        public void GetCollidersUnsafe(out Collider[] buffer, out int size)
        {
            buffer = this.buffer;
            size = bufferSize;
        }

        public void Play()
        {
            if (coroutine == null)
            {
                coroutine = StartCoroutine(UpdateColliders());
            }
        }

        public void Stop()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        public void AddListener(IColliderDetectionHandler handler)
        {
            listeners.Add(handler);
        }

        public void RemoveListener(IColliderDetectionHandler handler)
        {
            listeners.Remove(handler);
        }

        private IEnumerator UpdateColliders()
        {
            while (true)
            {
                var period = Random.Range(minScanPeriod, maxScanPeriod);
                yield return new WaitForSeconds(period);

                Array.Clear(buffer, 0, buffer.Length);
                bufferSize = Detect(buffer);
                
                InvokeCollidersUpdated(bufferSize, buffer);
            }
        }

        private void InvokeCollidersUpdated(int size, Collider[] buffer)
        {
            for (int i = 0, count = listeners.Count; i < count; i++)
            {
                var listener = listeners[i];
                listener.OnCollidersUpdated(buffer, size);
            }
            
            OnCollidersUpdated?.Invoke();
        }

        protected abstract int Detect(Collider[] buffer);
    }
}