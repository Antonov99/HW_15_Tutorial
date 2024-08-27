using System.Collections.Generic;
using Elementary;
using Game.GameEngine.Animation;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Combat/Combat State «Deal Damage By Animation Event»")]
    public sealed class UCombatState_DealDamageByAnimationEvent : MonoState
    {
        [SerializeField]
        public UCombatOperator combatOperator;

        [SerializeField]
        public UCombatAction_DealDamageIfAlive damageAction;

        [Space]
        [SerializeField]
        public UAnimatorMachine animationSystem;
        
        [Space]
        [SerializeField]
        public List<string> animationEvents = new()
        {
            "harvest"
        };

        public override void Enter()
        {
            animationSystem.OnStringReceived += OnAnimationEvent;
        }

        public override void Exit()
        {
            animationSystem.OnStringReceived -= OnAnimationEvent;
        }

        private void OnAnimationEvent(string message)
        {
            if (animationEvents.Contains(message) && combatOperator.IsActive)
            {
                damageAction.Do(combatOperator.Current);
            }
        }
    }
}