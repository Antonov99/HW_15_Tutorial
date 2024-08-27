using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    public abstract class UTakeDamageMechanics : MonoBehaviour
    {
        [SerializeField]
        public UTakeDamageEngine takeDamageEngine;
        
        private void OnEnable()
        {
            takeDamageEngine.OnDamageTaken += OnDamageTaken;
        }

        private void OnDisable()
        {
            takeDamageEngine.OnDamageTaken -= OnDamageTaken;
        }

        protected abstract void OnDamageTaken(TakeDamageArgs damageArgs);
    }
}