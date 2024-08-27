using System;
using System.Collections.Generic;
using Elementary;
using Game.GameEngine.Mechanics;
using Declarative;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Gameplay.Hero
{
    [Serializable]
    public sealed class HeroModel_States
    {
        [ShowInInspector, ReadOnly]
        public StateMachineAuto<HeroStateId> stateMachine = new();

        [Section]
        private IdleState idleState = new();

        [Section]
        private MoveState moveState = new();

        [Section]
        private CombatState combatState = new();

        [Section]
        private HarvestState harvestState = new();

        [Section]
        private DeathState deathState = new();

        private readonly BoolMechanics boolMechanics = new();

        private readonly FixedUpdateMechanics updateMechanics = new();

        [Construct]
        private void ConstructStateMachine(HeroModel_Core core)
        {
            stateMachine.states = new List<StateEntry<HeroStateId>>
            {
                new(HeroStateId.IDLE, idleState),
                new(HeroStateId.MOVE, moveState),
                new(HeroStateId.MELEE_COMBAT, combatState),
                new(HeroStateId.HARVEST_RESOURCE, harvestState),
                new(HeroStateId.DEATH, deathState)
            };

            stateMachine.orderedTransitions = new List<StateTransition<HeroStateId>>
            {
                new(HeroStateId.DEATH, () => core.life.hitPoints.IsOver()),
                new(HeroStateId.MOVE, () => core.move.moveMotor.IsMoving),
                new(HeroStateId.MELEE_COMBAT, () => core.combat.combatOperator.IsActive),
                new(HeroStateId.HARVEST_RESOURCE, () => core.harvest.harvestOperator.IsActive),
                new(HeroStateId.IDLE, () => true)
            };
        }

        [Construct]
        private void ConstructMechanics(HeroModel_Core core)
        {
            var enableVariable = core.main.isEnable;
            boolMechanics.Construct(enableVariable, isEnable =>
            {
                if (isEnable)
                    stateMachine.Enter();
                else
                    stateMachine.Exit();
            });

            updateMechanics.Construct(_ =>
            {
                if (enableVariable.Current)
                {
                    stateMachine.Update();
                }
            });
        }

        private sealed class IdleState : StateComposite
        {
        }

        private sealed class MoveState : StateComposite
        {
            private readonly MoveInDirectionState_SurfacePosition positionState = new();

            private readonly MoveInDirectionState_Rotation rotationState = new();

            [Construct]
            private void ConstructSelf()
            {
                states = new List<IState>
                {
                    positionState,
                    rotationState
                };
            }

            [Construct]
            private void ConstructStates(HeroModel_Core core)
            {
                positionState.ConstructMotor(core.move.moveMotor);
                positionState.ConstructTransform(core.main.transformEngine);
                positionState.ConstructSurface(core.main.walkableSurface);
                positionState.ConstructSpeed(core.move.fullSpeed);

                rotationState.mode = MoveInDirectionState_Rotation.Mode.SMOOTH;
                rotationState.rotationSpeed = 45.0f;
                rotationState.ConstructMotor(core.move.moveMotor);
                rotationState.ConstructTransform(core.main.transformEngine);
            }
        }

        private sealed class HarvestState : StateComposite
        {
            private readonly HarvestResourceState_TimeProgress progressState = new();

            [Construct]
            private void ConstructSelf()
            {
                states = new List<IState>
                {
                    progressState,
                };
            }

            [Construct]
            private void ConstructProgressState(ScriptableHero config, HeroModel_Core core)
            {
                progressState.ConstructOperator(core.harvest.harvestOperator);
                progressState.ConstructDuration(new Value<float>(config.harvestDuration));
            }
        }

        private sealed class CombatState : StateComposite
        {
            private readonly CombatState_ControlTargetDistance distanceState = new();

            private readonly CombatState_ControlTargetDestroy destroyState = new();
            
            private readonly CombatState_UpdateRotation updateRotationState = new();

            [Construct]
            private void ConstructSelf()
            {
                states = new List<IState>
                {
                    distanceState,
                    destroyState,
                    updateRotationState
                };
            }

            [Construct]
            private void ConstructStates(ScriptableHero config, GameObject attacker, HeroModel_Core core)
            {
                var combatOperator = core.combat.combatOperator;
                var transform = core.main.transformEngine;
                
                distanceState.ConstructOperator(combatOperator);
                distanceState.ConstructTransform(transform);
                distanceState.ConstructMinDistance(config.combatDistance);

                destroyState.ConstructOperator(combatOperator);
                destroyState.ConstructAttacker(attacker);
                
                //Control rotation:
                updateRotationState.ConstructOperator(combatOperator);
                updateRotationState.ConstructTransform(transform);
                updateRotationState.mode = CombatState_UpdateRotation.Mode.SMOOTH;
                updateRotationState.rotationSpeed = 45.0f;
            }
        }

        private sealed class DeathState : StateComposite
        {
        }
    }
}