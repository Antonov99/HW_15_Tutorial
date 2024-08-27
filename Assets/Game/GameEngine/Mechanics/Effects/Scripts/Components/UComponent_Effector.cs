using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Effects/Component «Effector»")]
    public sealed class UComponent_Effector : MonoBehaviour, IComponent_Effector
    {
        public event Action<IEffect> OnApplied
        {
            add { effector.OnApplied += value; }
            remove { effector.OnApplied -= value; }
        }

        public event Action<IEffect> OnDiscarded
        {
            add { effector.OnDiscarded += value; }
            remove { effector.OnDiscarded -= value; }
        }

        [SerializeField]
        private UEffector effector;
        
        public void Apply(IEffect effect)
        {
            effector.Apply(effect);
        }

        public void Discard(IEffect effect)
        {
            effector.Discard(effect);
        }
    }
}