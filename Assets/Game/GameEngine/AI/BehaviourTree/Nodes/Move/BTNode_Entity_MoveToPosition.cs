using System;
using AI.Blackboards;
using AI.BTree;
using Elementary;
using Entities;
using UnityEngine;

namespace Game.GameEngine.AI
{
    [Serializable]
    public sealed class BTNode_Entity_MoveToPosition : BehaviourNode
    {
        private readonly Agent_Entity_MoveToPosition moveAgent = new();

        private string unitKey;

        private string movePositionKey;

        private IBlackboard blackboard;
        
        public void ConstructBlackboardKeys(string unitKey, string movePositionKey)
        {
            this.unitKey = unitKey;
            this.movePositionKey = movePositionKey;
        }

        public void ConstructBlackboard(IBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public void ConstructStoppingDistance(float stoppingDistance)
        {
            moveAgent.SetStoppingDistance(stoppingDistance);
        }

        protected override void Run()
        {
            if (!blackboard.TryGetVariable(unitKey, out IEntity entity))
            {
                Return(false);
                return;
            }

            if (!blackboard.TryGetVariable(movePositionKey, out Vector3 targetPosition))
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