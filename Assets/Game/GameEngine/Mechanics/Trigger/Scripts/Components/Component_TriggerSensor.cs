using System;
using Elementary;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    public sealed class Component_TriggerSensor : IComponent_TriggerSensor
    {
        public event Action<Collider> OnEntered
        {
            add { sensor.OnTriggerEntered += value; }
            remove { sensor.OnTriggerEntered -= value; }
        }

        public event Action<Collider> OnStaying
        {
            add { sensor.OnTriggerStaying += value; }
            remove { sensor.OnTriggerStaying -= value; }
        }

        public event Action<Collider> OnExited
        {
            add { sensor.OnTriggerExited += value; }
            remove { sensor.OnTriggerExited -= value; }
        }

        private readonly TriggerSensor sensor;

        public Component_TriggerSensor(TriggerSensor sensor)
        {
            this.sensor = sensor;
        }
    }
}