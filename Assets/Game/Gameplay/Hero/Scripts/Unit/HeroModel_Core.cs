using System;
using Elementary;
using Game.GameEngine;
using Game.GameEngine.Mechanics;
using Declarative;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Gameplay.Hero
{
    [Serializable]
    public sealed class HeroModel_Core
    {
        [Section]
        public Main main;

        [Section]
        public Move move;

        [Section]
        public Harvest harvest;

        [Section]
        public Combat combat;

        [Section]
        public Life life;

        [Section]
        public Effects effects;

        [Serializable]
        public sealed class Main
        {
            public TransformEngine transformEngine = new();

            public BoolVariable isEnable = new();

            [NonSerialized]
            public Variable<IWalkableSurface> walkableSurface = new();
        }

        [Serializable]
        public sealed class Move
        {
            [ShowInInspector, ReadOnly]
            public MoveInDirectionMotor moveMotor;

            [Space]
            public FloatVariable baseSpeed;

            public FloatVariable multiplier;

            public FloatVariable fullSpeed;

            private readonly FixedUpdateMechanics updateMechanics = new();

            private readonly MoveSpeedConnector speedConnector = new();

            [Construct]
            private void ConstructMotor(Life life, Harvest harvest, Combat combat)
            {
                moveMotor.AddPrecondition(_ => life.hitPoints.IsExists());
                moveMotor.AddStartAction(_ => harvest.harvestOperator.Stop());
                moveMotor.AddStartAction(_ => combat.combatOperator.Stop());
            }

            [Construct]
            private void ConstructMechanics(Main main)
            {
                updateMechanics.Construct(_ =>
                {
                    if (main.isEnable.Current) moveMotor.Update();
                });

                speedConnector.Construct(baseSpeed, multiplier, fullSpeed);
            }

            [Construct]
            private void ConstructSpeed(ScriptableHero config)
            {
                baseSpeed.Current = config.baseSpeed;
                multiplier.Current = config.baseSpeedMultiplier;
            }
        }

        [Serializable]
        public sealed class Harvest
        {
            [ShowInInspector, ReadOnly]
            public readonly Operator<HarvestResourceOperation> harvestOperator = new();

            [Construct]
            private void ConstructHarvest(ScriptableHero config, Main main, Move move, Life life, Combat combat)
            {
                var transform = main.transformEngine;

                var checkDistance = new HarvestResourceCondition_CheckDistance(transform, config.harvestDistance);
                var checkTarget = new HarvestResourceCondition_CheckEntity(config.harvestConditions);
                
                harvestOperator.AddCondition(checkDistance);
                harvestOperator.AddCondition(checkTarget);
                harvestOperator.AddCondition(_ => life.hitPoints.IsExists());
                harvestOperator.AddCondition(_ => !move.moveMotor.IsMoving);
                harvestOperator.AddCondition(_ => !combat.combatOperator.IsActive);

                harvestOperator.AddStartAction(_ => combat.combatOperator.Stop());
                harvestOperator.AddStartAction(new HarvestResourceAction_LookAtResource(transform));

                harvestOperator.AddStopAction(new HarvestResourceAction_DestroyResourceIfCompleted());
            }
        }

        [Serializable]
        public sealed class Combat
        {
            [ShowInInspector, ReadOnly]
            public readonly Operator<CombatOperation> combatOperator = new();

            public IntVariable baseDamage;

            public FloatVariable multiplier;

            public IntVariable fullDamage;

            private readonly DamageConnector damageConnector = new();

            private CombatAction_DealDamageIfAlive damageAction = new();

            [Construct]
            private void ConstructCombat(ScriptableHero config, Main main, Move move, Life life, Harvest harvest)
            {
                var transformEngine = main.transformEngine;

                var checkEntity = new CombatCondition_CheckEntity(config.combatConditions);
                var checkDistance = new CombatCondition_CheckDistance(transformEngine, config.combatDistance);

                combatOperator.AddCondition(_ => !move.moveMotor.IsMoving);
                combatOperator.AddCondition(_ => life.hitPoints.IsExists());
                combatOperator.AddCondition(checkEntity);
                combatOperator.AddCondition(checkDistance);

                combatOperator.AddStartAction(new CombatAction_LookAtTarget(transformEngine));
                combatOperator.AddStartAction(_ => harvest.harvestOperator.Stop());
            }

            [Construct]
            private void ConstructDamageStats(ScriptableHero config)
            {
                damageConnector.Construct(baseDamage, multiplier, fullDamage);
                baseDamage.Current = config.baseDamage;
                multiplier.Current = config.baseDamageMultiplier;
            }

            [Construct]
            private void ConstructDamageAction(GameObject attacker)
            {
                damageAction.attacker = attacker;
                damageAction.damage = fullDamage;
            }

            public void DealDamage()
            {
                if (combatOperator.IsActive)
                {
                    damageAction.Do(combatOperator.Current);
                }
            }
        }

        [Serializable]
        public sealed class Life
        {
            [SerializeField]
            public HitPoints hitPoints = new();

            [SerializeField]
            public TakeDamageEngine takeDamageEngine = new();

            [ShowInInspector]
            public readonly Emitter<DestroyArgs> deathEmitter = new();

            [ShowInInspector]
            public readonly Emitter respawnEmitter = new();

            private readonly RestoreHitPointsMechanics restoreMechanics = new();

            [Construct]
            private void ConstructHitPoints(ScriptableHero config)
            {
                var hitPoints = config.baseHitPoints;
                this.hitPoints.Setup(hitPoints, hitPoints);
            }

            [Construct]
            private void ConstructRestoreMechanics()
            {
                restoreMechanics.SetDelay(2.5f);
                restoreMechanics.SetPeriod(1.0f);
                restoreMechanics.SetRestoreAtTime(1);
                restoreMechanics.Construct(hitPoints, takeDamageEngine);
            }

            [Construct]
            private void ConstructDeath(Combat combat, Harvest harvest, Move move)
            {
                deathEmitter.AddListener(_ => hitPoints.Setup(0, hitPoints.Max));
                deathEmitter.AddListener(_ => combat.combatOperator.Stop());
                deathEmitter.AddListener(_ => harvest.harvestOperator.Stop());
                deathEmitter.AddListener(_ => move.moveMotor.Interrupt());
            }

            [Construct]
            private void ConstructTakeDamage()
            {
                takeDamageEngine.Construct(hitPoints, deathEmitter);
            }

            [Construct]
            private void ConstructRespawn()
            {
                respawnEmitter.AddListener(hitPoints.RestoreToFull);
            }
        }

        [Serializable]
        public sealed class Effects
        {
            [ShowInInspector]
            public Effector<IEffect> effector = new();

            [Construct]
            private void Construct(Combat combat, Move move)
            {
                effector.AddHandler(new EffectHandler_MeleeDamage(combat.multiplier));
                effector.AddHandler(new EffectHandler_MoveSpeed(move.multiplier));
            }
        }
    }
}