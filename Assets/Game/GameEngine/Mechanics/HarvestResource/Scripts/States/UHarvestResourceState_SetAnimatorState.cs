using System;
using System.Runtime.Serialization;
using Sirenix.OdinInspector;
using Elementary;
using Game.GameEngine.Animation;
using Game.GameEngine.GameResources;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Harvest Resource/Harvest Resource State «Set Anim State»")]
    public sealed class UHarvestResourceState_SetAnimatorState : MonoState
    {
        [Space]
        [SerializeField]
        private UHarvestResourceOperator mechanics;

        [SerializeField]
        private UAnimatorMachine animatorEngine;

        [Space]
        [SerializeField]
        private Animation[] enterAnimations;

        [Space]
        [SerializeField]
        private bool hasExitAnimation;

        [ShowIf("hasExitAnimation")]
        [OptionalField]
        [SerializeField]
        private IntAdapter exitAnimation;

        public override void Enter()
        {
            var animationId = SelectAnimation();
            animatorEngine.ChangeState(animationId);
        }

        public override void Exit()
        {
            if (hasExitAnimation)
            {
                animatorEngine.ChangeState(exitAnimation.Current);
            }
        }

        private int SelectAnimation()
        {
            var operation = mechanics.Current;
            var resourceType = operation
                .targetResource
                .Get<IComponent_GetResourceType>()
                .Type;

            for (int i = 0, count = enterAnimations.Length; i < count; i++)
            {
                var animation = enterAnimations[i];
                if (animation.resourceType == resourceType)
                {
                    return animation.info.Current;
                }
            }

            throw new Exception($"Animation is not found {resourceType}!");
        }

        [Serializable]
        private struct Animation
        {
            [SerializeField]
            public ResourceType resourceType;

            [SerializeField]
            public IntAdapter info;
        }
    }
}