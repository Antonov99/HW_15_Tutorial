using UnityEngine;

namespace Elementary
{
    public abstract class ColliderDetectionObserver : MonoBehaviour
    {
        [SerializeField]
        private ColliderDetection sensor;

        protected virtual void OnEnable()
        {
            sensor.OnCollidersUpdated += OnCollidersUpdated;
        }

        protected virtual void OnDisable()
        {
            sensor.OnCollidersUpdated -= OnCollidersUpdated;
        }

        protected void OnCollidersUpdated()
        {
            sensor.GetCollidersUnsafe(out var buffer, out var size);
            OnCollidersUpdated(buffer, size);
        }

        protected abstract void OnCollidersUpdated(Collider[] buffer, int size);
    }
}