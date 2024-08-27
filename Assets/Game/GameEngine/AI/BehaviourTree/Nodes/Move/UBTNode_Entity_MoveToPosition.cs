using AI.Blackboards;
using AI.BTree;
using Elementary;
using Entities;
using UnityEngine;

namespace Game.GameEngine.AI
{
    [AddComponentMenu(BehaviourTreePaths.MENU_PATH + "BTNode «Move To Position» (Entity)")]
    public sealed class UBTNode_Entity_MoveToPosition : UnityBehaviourNode, IBlackboardInjective
    {
        public IBlackboard Blackboard { private get; set; }

        [Space]
        [SerializeField]
        private FloatAdapter stoppingDistance; //0.15f;

        [Space]
        [BlackboardKey]
        [SerializeField]
        private string unitKey;

        [BlackboardKey]
        [SerializeField]
        private string movePositionKey;

        private Agent_Entity_MoveToPosition moveAgent;

        private void Awake()
        {
            moveAgent = new Agent_Entity_MoveToPosition();
            moveAgent.SetStoppingDistance(stoppingDistance.Current);
        }

        protected override void Run()
        {
            if (!Blackboard.TryGetVariable(unitKey, out IEntity entity))
            {
                Return(false);
                return;
            }

            if (!Blackboard.TryGetVariable(movePositionKey, out Vector3 targetPosition))
            {
                Return(false);
                return;
            }

            moveAgent.OnTargetReached += OnTargetReached;
            moveAgent.SetMovingEntity(entity);
            moveAgent.SetTarget(targetPosition);
            moveAgent.Play();
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
            moveAgent.OnTargetReached -= OnTargetReached;
            moveAgent.Stop();
        }
    }
}