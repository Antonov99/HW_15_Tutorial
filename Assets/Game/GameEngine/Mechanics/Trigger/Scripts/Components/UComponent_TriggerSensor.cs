using System;
using Elementary;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Trigger/Component «Trigger Sensor»")]
    public sealed class UComponent_TriggerSensor : MonoBehaviour, IComponent_TriggerSensor
    {
        public event Action<Collider> OnEntered
        {
            add { eventReceiver.OnTriggerEntered += value; }
            remove { eventReceiver.OnTriggerEntered -= value; }
        }

        public event Action<Collider> OnStaying
        {
            add { eventReceiver.OnTriggerStaying += value; }
            remove { eventReceiver.OnTriggerStaying -= value; }
        }

        public event Action<Collider> OnExited
        {
            add { eventReceiver.OnTriggerExited += value; }
            remove { eventReceiver.OnTriggerExited -= value; }
        }

        [SerializeField]
        private TriggerSensor eventReceiver;
    }
}