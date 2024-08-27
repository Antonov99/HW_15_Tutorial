using System;
using Elementary;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Collision/Component «Collision Sensor»")]
    public sealed class UComponent_CollisionSensor : MonoBehaviour, IComponent_CollisionSensor
    {
        public event Action<Collision> OnCollisionEntered
        {
            add { eventReceiver.OnCollisionEntered += value; }
            remove { eventReceiver.OnCollisionEntered -= value; }
        }

        public event Action<Collision> OnCollisionStaying
        {
            add { eventReceiver.OnCollisionStaying += value; }
            remove { eventReceiver.OnCollisionStaying -= value; }
        }

        public event Action<Collision> OnCollisionExited
        {
            add { eventReceiver.OnCollisionExited += value; }
            remove { eventReceiver.OnCollisionExited -= value; }
        }

        [SerializeField]
        private CollisionSensor eventReceiver;
    }
}