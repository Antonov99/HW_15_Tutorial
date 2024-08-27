using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/TakeDamage/Take Damage Engine")]
    public sealed class UTakeDamageEngine : MonoBehaviour
    {
        public event Action<TakeDamageArgs> OnDamageTaken;

        [SerializeField]
        private UHitPoints hitPointsEngine;

        [SerializeField]
        private DestroyEventReceiver destroyReceiver;

        [Button]
        [GUIColor(0, 1, 0)]
        public void TakeDamage(TakeDamageArgs damageArgs)
        {
            if (hitPointsEngine.Current <= 0)
            {
                return;
            }

            hitPointsEngine.Current -= damageArgs.damage;
            OnDamageTaken?.Invoke(damageArgs);

            if (hitPointsEngine.Current <= 0)
            {
                var destroyEvent = MechanicsUtils.ConvertToDestroyEvent(damageArgs);
                destroyReceiver.Call(destroyEvent);
            }
        }
    }
}