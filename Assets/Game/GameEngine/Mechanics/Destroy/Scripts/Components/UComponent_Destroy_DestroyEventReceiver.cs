using System;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Destroy/Component «Destroy» (Destroy Event Receiver)")]
    public sealed class UComponent_Destroy_DestroyEventReceiver : MonoBehaviour,
        IComponent_Destroy<DestroyArgs>,
        IComponent_OnDestroyed<DestroyArgs>
    {
        public event Action<DestroyArgs> OnDestroyed
        {
            add { eventReceiver.OnDestroy += value; }
            remove { eventReceiver.OnDestroy -= value; }
        }

        [SerializeField]
        private DestroyEventReceiver eventReceiver;

        public void Destroy(DestroyArgs args)
        {
            eventReceiver.Call(args);
        }
    }
}