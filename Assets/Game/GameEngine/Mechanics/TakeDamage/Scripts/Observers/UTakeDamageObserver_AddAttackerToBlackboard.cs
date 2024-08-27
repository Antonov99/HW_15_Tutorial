using AI.Blackboards;
using Entities;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/TakeDamage/Take Damage Observer «Add Attacker To Blackboard»")]
    public sealed class UTakeDamageObserver_AddAttackerToBlackboard : UTakeDamageObserver,
        IBlackboardInjective
    {
        public IBlackboard Blackboard { private get; set; }

        [Space]
        [BlackboardKey]
        [SerializeField]
        public string attackerKey;

        protected override void OnDamageTaken(TakeDamageArgs damageArgs)
        {
            if (damageArgs.reason != TakeDamageReason.MELEE)
            {
                return;
            }

            if (Blackboard.HasVariable(attackerKey))
            {
                return;
            }

            if (damageArgs.source is IEntity attacker)
            {
                Blackboard.AddVariable(attackerKey, attacker);
            }
        }
    }
}