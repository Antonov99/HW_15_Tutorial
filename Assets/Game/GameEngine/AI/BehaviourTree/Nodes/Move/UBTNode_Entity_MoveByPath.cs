using AI.Blackboards;
using AI.BTree;
using Elementary;
using Entities;
using UnityEngine;

namespace Game.GameEngine.AI
{
    [AddComponentMenu(BehaviourTreePaths.MENU_PATH + "BTNode «Move By Path» (Entity)")]
    public sealed class UBTNode_Entity_MoveByPath : UnityBehaviourNode, IBlackboardInjective
    {
        public IBlackboard Blackboard { private get; set; }

        [Space]
        [BlackboardKey]
        [SerializeField]
        private string movePathKey;
        
        [BlackboardKey]
        [SerializeField]
        private string unitKey;

        [Space]
        [SerializeField]
        private FloatAdapter stoppingDistance;

        private Agent_Entity_MoveByPoints moveAgent;

        private void Awake()
        {
            moveAgent = new Agent_Entity_MoveByPoints();
            moveAgent.SetStoppingDistance(stoppingDistance.Current);
        }

        protected override void Run()
        {
            if (!Blackboard.TryGetVariable(unitKey, out IEntity unit))
            {
                Debug.LogWarning("Unit is not found!");
                Return(false);
                return;
            }

            if (!Blackboard.TryGetVariable(movePathKey, out Vector3[] movePositions))
            {
                Debug.LogWarning("Move path is not found!");
                Return(false);
                return;
            }

            moveAgent.OnPathFinished += OnMoveFinished;
            moveAgent.SetMovingEntity(unit);
            moveAgent.SetPath(movePositions);
            moveAgent.Play();
        }

        private void OnMoveFinished()
        {
            Return(true);
        }

        protected override void OnDispose()
        {
            moveAgent.OnPathFinished -= OnMoveFinished;
            moveAgent.Stop();
        }
    }
}