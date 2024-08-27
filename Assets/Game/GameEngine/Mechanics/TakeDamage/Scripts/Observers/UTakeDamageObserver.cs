using Entities;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    public abstract class UTakeDamageObserver : MonoBehaviour
    {
        [SerializeField]
        public MonoEntity unit;
        
        private IComponent_OnDamageTaken takeDamageComponent;

        private void OnEnable()
        {
            takeDamageComponent = unit.Get<IComponent_OnDamageTaken>(); 
            takeDamageComponent.OnDamageTaken += OnDamageTaken;
        }

        private void OnDisable()
        {
            takeDamageComponent.OnDamageTaken -= OnDamageTaken;
            takeDamageComponent = null;
        }

        protected abstract void OnDamageTaken(TakeDamageArgs obj);
    }
}