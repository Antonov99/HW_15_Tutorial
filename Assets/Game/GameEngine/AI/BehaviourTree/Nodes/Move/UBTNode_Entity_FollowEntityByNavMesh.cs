using AI.Blackboards;
using AI.BTree;
using Elementary;
using Entities;
using UnityEngine;
using UnityEngine.AI;

namespace Game.GameEngine.AI
{
    [AddComponentMenu(BehaviourTreePaths.MENU_PATH + "BTNode «Follow Entity By NavMesh» (Entity)")]
    public sealed class UBTNode_Entity_FollowEntityByNavMesh : UnityBehaviourNode, IBlackboardInjective
    {
        public IBlackboard Blackboard { private get; set; }

        [Space]
        [SerializeField]
        private FloatAdapter stoppingDistance; // 0.15f;

        [SerializeField]
        private FloatAdapter minPointDistance; //0.15f;

        [Space]
        [BlackboardKey]
        [SerializeField]
        private string unitKey;

        [BlackboardKey]
        [SerializeField]
        private string targetKey;

        private Agent_Entity_FollowEntityByNavMesh followAgent;

        private void Awake()
        {
            followAgent = new Agent_Entity_FollowEntityByNavMesh();
            followAgent.SetNavMeshAreas(NavMesh.AllAreas);
            followAgent.SetStoppingDistance(stoppingDistance.Current);
            followAgent.SetMinPointDistance(minPointDistance.Current);
            followAgent.SetCalculatePathPeriod(new WaitForFixedUpdate());
            followAgent.SetCheckTargetReachedPeriod(null);
        }

        protected override void Run()
        {
            if (!Blackboard.TryGetVariable(unitKey, out IEntity unit))
            {
                Return(false);
                return;
            }

            if (!Blackboard.TryGetVariable(targetKey, out IEntity target))
            {
                Return(false);
                return;
            }

            followAgent.OnTargetReached += OnTargetReached;
            followAgent.SetTargetEntity(target);
            followAgent.SetFollowingEntity(unit);
            followAgent.Play();
        }

        private void OnTargetReached(bool isReached)
        {
            if (isReached)
            {
                Return(true);
            }
        }

        protected override void OnDispose()
        {
            followAgent.Stop();
            followAgent.OnTargetReached -= OnTargetReached;
        }
    }
}