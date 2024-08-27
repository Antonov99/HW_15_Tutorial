using System.Collections.Generic;
using Elementary;
using Game.GameEngine.Animation;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Harvest Resource/Harvest Resource State «Hit Resource By Anim Event»")]
    public sealed class UHarvestResourceState_HitResourceByAnimationEvent : MonoState
    {
        [SerializeField]
        public UHarvestResourceOperator harvestEngine;

        [SerializeField]
        public UHarvestResourceAction_HitResource hitAction;

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
            if (animationEvents.Contains(message) && harvestEngine.IsActive)
            {
                hitAction.Do(harvestEngine.Current);
            }
        }
    }
}