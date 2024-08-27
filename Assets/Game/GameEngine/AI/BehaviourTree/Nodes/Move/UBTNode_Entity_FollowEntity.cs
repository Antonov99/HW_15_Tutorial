using AI.Blackboards;
using AI.BTree;
using Elementary;
using Entities;
using UnityEngine;

namespace Game.GameEngine.AI
{
    [AddComponentMenu(BehaviourTreePaths.MENU_PATH + "BTNode «Follow Entity» (Entity)")]
    public sealed class UBTNode_Entity_FollowEntity : UnityBehaviourNode, IBlackboardInjective
    {
        public IBlackboard Blackboard { private get; set; }

        [Space]
        [BlackboardKey]
        [SerializeField]
        private string unitKey;

        [BlackboardKey]
        [SerializeField]
        private string targetKey;

        [BlackboardKey]
        [SerializeField]
        private string stoppingDistanceKey;

        private Agent_Entity_FollowEntity followAgent;
        
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

            if (!Blackboard.TryGetVariable(stoppingDistanceKey, out float stoppingDistance))
            {
                Return(false);
                return;
            }

            followAgent.OnTargetReached += OnTargetReached;
            followAgent.SetFollowingEntity(unit);
            followAgent.SetTargetEntity(target);
            followAgent.SetStoppingDistance(stoppingDistance);
            followAgent.Play();
        }

        private void OnTargetReached(bool isReached)
        {
            if (isReached)
            {
                Return(true);
            }
        }

        private void Awake()
        {
            followAgent = new Agent_Entity_FollowEntity();
        }

        protected override void OnDispose()
        {
            followAgent.OnTargetReached -= OnTargetReached;
            followAgent.Stop();
        }
    }
}