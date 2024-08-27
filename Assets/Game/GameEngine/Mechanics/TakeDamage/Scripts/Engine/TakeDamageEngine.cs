using System;
using Elementary;

namespace Game.GameEngine.Mechanics
{
    [Serializable]
    public sealed class TakeDamageEngine : Emitter<TakeDamageArgs>
    {
        private IHitPoints hitPointsEngine;

        private IEmitter<DestroyArgs> destroyReceiver;

        public void Construct(IHitPoints hitPointsEngine, IEmitter<DestroyArgs> destroyReceiver)
        {
            this.hitPointsEngine = hitPointsEngine;
            this.destroyReceiver = destroyReceiver;
        }
        
        public override void Call(TakeDamageArgs damageArgs)
        {
            if (damageArgs.damage <= 0)
            {
                return;
            }
        
            if (hitPointsEngine.Current <= 0)
            {
                return;
            }

            hitPointsEngine.Current -= damageArgs.damage;
            base.Call(damageArgs);

            if (hitPointsEngine.Current <= 0)
            {
                var destroyEvent = MechanicsUtils.ConvertToDestroyEvent(damageArgs);
                destroyReceiver.Call(destroyEvent);
            }
        }
    }
}