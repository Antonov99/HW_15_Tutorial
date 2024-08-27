using System;
using System.Collections.Generic;
using AI.Blackboards;
using AI.BTree;
using AI.Iterators;
using AI.Waypoints;
using Elementary;
using Entities;
using Game.GameEngine.AI;
using Game.GameEngine.Mechanics;
using GameSystem;
using Declarative;
using Polygons;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Gameplay.Enemies
{
    public sealed class EnemyWarriorAI : DeclarativeModel, IGameConstructElement
    {
        [Section]
        [SerializeField]
        private ScriptableEnemyWarriorAI config;

        [Section]
        [SerializeField]
        private Core core = new();

        [Section]
        [SerializeField]
        private Components components = new();

        [Section]
        [ShowInInspector, ReadOnly]
        private Behaviour behaviour = new();

        [SerializeField]
        private External external;

        [Serializable]
        public sealed class Core
        {
            [SerializeField]
            public BoolVariable isEnable = new();

            [ShowInInspector, ReadOnly]
            public Blackboard blackboard = new();

            [Space]
            [SerializeField]
            public ColliderDetectionOverlapSphere sensor;

            private readonly BoolMechanics enableMechanics = new();

            [Construct]
            private void ConstructEnable()
            {
                enableMechanics.Construct(this.isEnable, isEnable =>
                {
                    if (isEnable)
                        sensor.Play();
                    else
                        sensor.Stop();
                });
            }

            [Construct]
            private void ConstructSensor(ScriptableEnemyWarriorAI config)
            {
                var opponentDetector = new EnemyOpponentDetector(
                    blackboard,
                    ScriptableEnemyWarriorAI.TARGET_KEY,
                    config.detectTargetConditions
                );
                sensor.AddListener(opponentDetector);
            }
        }

        [Serializable]
        private sealed class Components
        {
            [SerializeField]
            private MonoEntityStd ai;

            [Construct]
            private void Construct(Core core)
            {
                ai.Add(new Component_Enable(core.isEnable));
                ai.Add(new Component_Blackboard(core.blackboard));
            }
        }

        private sealed class Behaviour
        {
            private readonly BehaviourTree behaviourTree = new();

            [Section]
            [ShowInInspector, ReadOnly]
            private AttackBranch attackBranch = new();

            [Section]
            [ShowInInspector, ReadOnly]
            private PatrolBranch patrolBranch = new();

            private readonly BehaviourTreeAborter_ByBlackboard treeAborter = new();

            private readonly UpdateMechanics updateMechanics = new();

            [Construct]
            private void ConstructTree()
            {
                behaviourTree.root = new BehaviourNodeSelector(
                    attackBranch,
                    patrolBranch
                );
            }

            [Construct]
            private void ConstructUpdateMechanics(Core core)
            {
                updateMechanics.Construct(_ =>
                {
                    if (core.isEnable.Current)
                    {
                        behaviourTree.Run(null);
                    }
                    else
                    {
                        behaviourTree.Abort();
                    }
                });
            }

            [Construct]
            private void ConstructAbort(Core core)
            {
                treeAborter.tree = behaviourTree;
                treeAborter.blackboard = core.blackboard;
                treeAborter.blackboardKeys = new List<string>
                {
                    ScriptableEnemyWarriorAI.TARGET_KEY
                };
            }

            public sealed class AttackBranch : BehaviourNodeSequence
            {
                [ShowInInspector, ReadOnly]
                private BehaviourNodeCondition conditionNode = new();

                [ShowInInspector, ReadOnly]
                private BTNode_Entity_FollowEntityByPolygon followNode = new();

                [ShowInInspector, ReadOnly]
                private BehaviourNode_WaitForSeconds waitForSeconds = new();

                [ShowInInspector, ReadOnly]
                private BTNode_Entity_MeleeCombat combatNode = new();

                [Construct]
                private void ConstructSelf()
                {
                    children = new IBehaviourNode[]
                    {
                        conditionNode,
                        followNode,
                        waitForSeconds,
                        new BehaviourNodeDecorator(combatNode, success: true)
                    };
                }

                [Construct]
                private void ConstructConditionNode(Core core)
                {
                    conditionNode.condition = new BTCondition_HasBlackboardVariable(
                        core.blackboard, ScriptableEnemyWarriorAI.TARGET_KEY
                    );
                }

                [Construct]
                private void ConstructFollowNode(ScriptableEnemyWarriorAI config, MonoBehaviour monoContext, Core core)
                {
                    followNode.ConstructBlackboard(core.blackboard);
                    followNode.ConstructBlackboardKeys(
                        ScriptableEnemyWarriorAI.UNIT_KEY,
                        ScriptableEnemyWarriorAI.TARGET_KEY,
                        ScriptableEnemyWarriorAI.SURFACE_KEY
                    );
                    followNode.ConstructIntermediateDistance(config.pointStoppingDistance);
                    followNode.ConstructStoppingDistance(config.meleeStoppingDistance);
                }

                [Construct]
                private void ConstructWaitNode(MonoBehaviour monoContext)
                {
                    waitForSeconds.waitSeconds = 0.1f;
                }

                [Construct]
                private void ConstructCombatNode(Core core)
                {
                    combatNode.ConstructBlackboard(core.blackboard);
                    combatNode.ConstructBlackboardKeys(
                        ScriptableEnemyWarriorAI.UNIT_KEY,
                        ScriptableEnemyWarriorAI.TARGET_KEY
                    );
                }
            }

            public sealed class PatrolBranch : BehaviourNodeSequence
            {
                [ShowInInspector, ReadOnly]
                private readonly BTNode_Iterator_AssignPosition assignPositionNode = new();

                [ShowInInspector, ReadOnly]
                private readonly BTNode_Iterator_MoveNext moveNextPointNode = new();

                [ShowInInspector, ReadOnly]
                private readonly BTNode_Entity_MoveToPosition moveToPositionNode = new();

                [ShowInInspector, ReadOnly]
                private readonly BehaviourNode_WaitForSeconds waitNode = new();

                [Construct]
                private void ConstructSelf()
                {
                    children = new IBehaviourNode[]
                    {
                        assignPositionNode,
                        moveNextPointNode,
                        moveToPositionNode,
                        waitNode
                    };
                }

                [Construct]
                private void ConstructAssignPositionNode(Core core)
                {
                    assignPositionNode.ConstructBlackboard(core.blackboard);
                    assignPositionNode.ConstructBlackboardKeys(
                        ScriptableEnemyWarriorAI.WAYPOINTS_KEY,
                        ScriptableEnemyWarriorAI.TARGET_POSITION_KEY
                    );
                }

                [Construct]
                private void ConstructMoveNextNode(Core core)
                {
                    moveNextPointNode.ConstructBlackboard(core.blackboard);
                    moveNextPointNode.ConstructBlackboardKeys(ScriptableEnemyWarriorAI.WAYPOINTS_KEY);
                }

                [Construct]
                private void ConstructMoveToPositionNode(Core core, ScriptableEnemyWarriorAI config)
                {
                    moveToPositionNode.ConstructBlackboard(core.blackboard);
                    moveToPositionNode.ConstructBlackboardKeys(
                        ScriptableEnemyWarriorAI.UNIT_KEY,
                        ScriptableEnemyWarriorAI.TARGET_POSITION_KEY
                    );
                    moveToPositionNode.ConstructStoppingDistance(config.pointStoppingDistance);
                }

                [Construct]
                private void ConstructWaitNode(ScriptableEnemyWarriorAI config)
                {
                    waitNode.waitSeconds = config.patrolWaitTime;
                }
            }
        }

        [Serializable]
        public sealed class External
        {
            [SerializeField]
            public MonoEntity unit;

            [Space]
            [SerializeField]
            public WaypointsPath waypointsPath;

            [SerializeField]
            public IteratorMode waypointMode = IteratorMode.CIRCLE;

            [Space]
            [SerializeField]
            public string surfacePolygonName = "WoodPolygon";

            // ReSharper disable once UnusedParameter.Global
            public void ConstructGame(EnemyWarriorAI ai, GameContext context)
            {
                var blackboard = ai.core.blackboard;
                var sensor = ai.core.sensor;

                //Set Unit:
                blackboard.AddVariable(ScriptableEnemyWarriorAI.UNIT_KEY, unit);

                //Set Waypoints:
                var waypoints = waypointsPath.GetPositionPoints().ToArray();
                var iterator = IteratorFactory.CreateIterator(waypointMode, waypoints);
                blackboard.AddVariable(ScriptableEnemyWarriorAI.WAYPOINTS_KEY, iterator);

                //Set surface:
                var polygon = GameObject.Find(surfacePolygonName).GetComponent<MonoPolygon>();
                blackboard.AddVariable(ScriptableEnemyWarriorAI.SURFACE_KEY, polygon);

                //Set center point:
                var centerPoint = unit.Get<IComponent_GetPivot>().Pivot;
                sensor.SetCenterPoint(centerPoint);
            }
        }

        void IGameConstructElement.ConstructGame(GameContext context)
        {
            external.ConstructGame(this, context);
        }
    }
}