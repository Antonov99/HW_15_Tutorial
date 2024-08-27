using System;
using Elementary;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    public sealed class Component_ColliderSensor : IComponent_ColliderSensor
    {
        public event Action OnCollisionsUpdated
        {
            add { sensor.OnCollidersUpdated += value; }
            remove { sensor.OnCollidersUpdated -= value; }
        }

        private readonly ColliderDetection sensor;

        public Component_ColliderSensor(ColliderDetection sensor)
        {
            this.sensor = sensor;
        }

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