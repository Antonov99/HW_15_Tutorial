using System;
using Game.GameEngine.Mechanics;
using UnityEngine;

namespace Game.GameEngine
{
    [AddComponentMenu("GameEngine/Mechanics/TakeDamage/Component «Take Damage»")]
    public sealed class UComponent_TakeDamage : MonoBehaviour, 
        IComponent_TakeDamage,
        IComponent_OnDamageTaken
    {
        public event Action<TakeDamageArgs> OnDamageTaken
        {
            add { engine.OnDamageTaken += value; }
            remove { engine.OnDamageTaken -= value; }
        }

        [SerializeField]
        private UTakeDamageEngine engine;
        
        public void TakeDamage(TakeDamageArgs damageArgs)
        {
            engine.TakeDamage(damageArgs);
        }
    }
}