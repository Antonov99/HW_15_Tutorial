using Elementary;
using Game.GameEngine.Animation;
using Game.GameEngine.GameResources;
using Game.GameEngine.Mechanics;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Gameplay.Hero
{
    public sealed class UHeroAnimatorMachine : UAnimatorMachine
    {
        [Space]
        [SerializeField]
        private ScriptableInt idleStateId;

        [SerializeField]
        private ScriptableInt moveStateId;

        [SerializeField]
        private ScriptableInt chopStateId;

        [SerializeField]
        private ScriptableInt attackStateId;

        [SerializeField]
        private ScriptableInt mineStateId;

        [SerializeField]
        private ScriptableInt dieStateId;

        [Title("References")]
        [SerializeField]
        private UHeroStateMachine stateMachine;

        [SerializeField]
        private UHarvestResourceOperator harvestEngine;

        protected override void OnEnable()
        {
            base.OnEnable();
            stateMachine.OnStateSwitched += OnStateChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            stateMachine.OnStateSwitched -= OnStateChanged;
        }

        private void OnStateChanged(HeroStateId state)
        {
            if (state == HeroStateId.IDLE)
            {
                ChangeState(idleStateId.Current);
            }
            else if (state == HeroStateId.MOVE)
            {
                ChangeState(moveStateId.Current);
            }
            else if (state == HeroStateId.HARVEST_RESOURCE)
            {
                ChangeState(SelectHarvestAnimation());
            }
            else if (state == HeroStateId.MELEE_COMBAT)
            {
                ChangeState(attackStateId.Current);
            }
            else if (state == HeroStateId.DEATH)
            {
                ChangeState(dieStateId.Current);
            }
        }

        private int SelectHarvestAnimation()
        {
            var operation = harvestEngine.Current;
            var resourceType = operation.resourceType;
            if (resourceType == ResourceType.WOOD)
            {
                return chopStateId.Current;
            }

            if (resourceType == ResourceType.STONE)
            {
                return mineStateId.Current;
            }

            //By default:
            return chopStateId.Current;
        }
    }
}