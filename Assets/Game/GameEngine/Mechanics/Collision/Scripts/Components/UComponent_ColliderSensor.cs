using System;
using Elementary;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Collision/Component «Collider Sensor»")]
    public sealed class UComponent_ColliderSensor : MonoBehaviour, IComponent_ColliderSensor
    {
        public event Action OnCollisionsUpdated
        {
            add { sensor.OnCollidersUpdated += value; }
            remove { sensor.OnCollidersUpdated -= value; }
        }

        [SerializeField]
        private ColliderDetection sensor;
        
        public void GetCollidersNonAlloc(Collider[] buffer, out int size)
        {
            sensor.GetCollidersNonAlloc(buffer, out size);
        }

        public void GetCollidersUnsafe(out Collider[] buffer, out int size)
        {
            sensor.GetCollidersUnsafe(out buffer, out size);
        }
    }
}