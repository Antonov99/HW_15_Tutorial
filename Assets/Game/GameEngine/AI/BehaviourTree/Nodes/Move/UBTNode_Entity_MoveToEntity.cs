using AI.Blackboards;
using AI.BTree;
using Elementary;
using Entities;
using Game.GameEngine.Mechanics;
using UnityEngine;

namespace Game.GameEngine.AI
{
    [AddComponentMenu(BehaviourTreePaths.MENU_PATH + "BTNode «Move To Entity» (Entity)")]
    public sealed class UBTNode_Entity_MoveToEntity : UnityBehaviourNode, IBlackboardInjective
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
        private string targetKey;
        
        private Agent_Entity_MoveToPosition moveAgent;

        private void Awake()
        {
            moveAgent = new Agent_Entity_MoveToPosition();
            moveAgent.SetStoppingDistance(stoppingDistance.Current);
        }

        protected override void Run()
        {
            if (!Blackboard.TryGetVariable(unitKey, out IEntity myUnit))
            {
                Return(false);
                return;
            }

            if (!Blackboard.TryGetVariable(targetKey, out IEntity targetUnit))
            {
                Return(false);
                return;
            }

            moveAgent.OnTargetReached += OnTargetReached;
            moveAgent.SetMovingEntity(myUnit);

            var targetPosition = targetUnit.Get<IComponent_GetPosition>().Position;
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