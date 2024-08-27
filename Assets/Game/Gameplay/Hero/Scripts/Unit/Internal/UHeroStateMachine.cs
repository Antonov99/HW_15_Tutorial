using System.Runtime.Serialization;
using Elementary;
using Game.GameEngine.Mechanics;
using UnityEngine;

namespace Game.Gameplay.Hero
{
    public sealed class UHeroStateMachine : MonoStateMachine<HeroStateId>
    {
        [Space]
        [OptionalField]
        [SerializeField]
        private UMoveInDirectionMotor moveEngine;

        [OptionalField]
        [SerializeField]
        private UCombatOperator combatOperator;

        [OptionalField]
        [SerializeField]
        private UHarvestResourceOperator harvestEngine;

        [OptionalField]
        [SerializeField]
        private DestroyEventReceiver destroyReceiver;

        [OptionalField]
        [SerializeField]
        private MonoEmitter respawnReceiver;

        public override void Enter()
        {
            if (moveEngine != null)
            {
                moveEngine.OnStartMove += OnMoveStarted;
                moveEngine.OnStopMove += OnMoveEnded;
            }

            if (combatOperator != null)
            {
                combatOperator.OnStarted += OnCombatStarted;
                combatOperator.OnStopped += OnCombatEnded;
            }

            if (harvestEngine != null)
            {
                harvestEngine.OnStarted += OnHarvestStarted;
                harvestEngine.OnStopped += OnHarvestEnded;
            }

            if (destroyReceiver != null)
            {
                destroyReceiver.OnDestroy += OnDied;
            }

            if (respawnReceiver != null)
            {
                respawnReceiver.OnEvent += OnRespawned;
            }

            base.Enter();
        }

        public override void Exit()
        {
            if (moveEngine != null)
            {
                moveEngine.OnStartMove -= OnMoveStarted;
                moveEngine.OnStopMove -= OnMoveEnded;
            }

            if (combatOperator != null)
            {
                combatOperator.OnStarted -= OnCombatStarted;
                combatOperator.OnStopped -= OnCombatEnded;
            }

            if (harvestEngine != null)
            {
                harvestEngine.OnStarted -= OnHarvestStarted;
                harvestEngine.OnStopped -= OnHarvestEnded;
            }

            if (destroyReceiver != null)
            {
                destroyReceiver.OnDestroy -= OnDied;
            }

            if (respawnReceiver != null)
            {
                respawnReceiver.OnEvent -= OnRespawned;
            }

            base.Exit();
        }

        #region MechanicsEvents

        private void OnDied(DestroyArgs destroyArgs)
        {
            SwitchState(HeroStateId.DEATH);
        }

        private void OnMoveStarted()
        {
            SwitchState(HeroStateId.MOVE);
        }

        private void OnMoveEnded()
        {
            if (CurrentState == HeroStateId.MOVE)
            {
                SwitchState(HeroStateId.IDLE);
            }
        }

        private void OnHarvestStarted(HarvestResourceOperation operation)
        {
            SwitchState(HeroStateId.HARVEST_RESOURCE);
        }

        private void OnHarvestEnded(HarvestResourceOperation operation)
        {
            if (CurrentState == HeroStateId.HARVEST_RESOURCE)
            {
                SwitchState(HeroStateId.IDLE);
            }
        }

        private void OnCombatStarted(CombatOperation operation)
        {
            SwitchState(HeroStateId.MELEE_COMBAT);
        }

        private void OnCombatEnded(CombatOperation operation)
        {
            if (CurrentState == HeroStateId.MELEE_COMBAT)
            {
                SwitchState(HeroStateId.IDLE);
            }
        }

        private void OnRespawned()
        {
            if (CurrentState == HeroStateId.DEATH)
            {
                SwitchState(HeroStateId.IDLE);
            }
        }

        #endregion
    }
}