using System;
using System.Collections.Generic;
using AI.Blackboards;
using AI.BTree;
using UnityEngine;

namespace Game.GameEngine.AI
{
    [Serializable]
    public sealed class BTNode_Iterator_AssignPosition : BehaviourNode
    {
        private IBlackboard blackboard;

        private string iteratorKey;

        private string positionKey;

        public void ConstructBlackboard(IBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public void ConstructBlackboardKeys(string iteratorKey, string positionKey)
        {
            this.iteratorKey = iteratorKey;
            this.positionKey = positionKey;
        }

        protected override void Run()
        {
            if (!blackboard.TryGetVariable(iteratorKey, out IEnumerator<Vector3> iterator))
            {
                Return(false);
                return;
            }

            blackboard.ReplaceVariable(positionKey, iterator.Current);
            Return(true);
        }
    }
}