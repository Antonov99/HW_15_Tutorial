using System.Collections.Generic;
using AI.Blackboards;
using AI.BTree;
using UnityEngine;

namespace Game.GameEngine.AI
{
    public sealed class BTNode_Iterator_MoveNext : BehaviourNode
    {
        private string iteratorKey;

        private IBlackboard blackboard;

        public void ConstructBlackboard(IBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public void ConstructBlackboardKeys(string iteratorKey)
        {
            this.iteratorKey = iteratorKey;
        }
        
        protected override void Run()
        {
            if (!blackboard.TryGetVariable(iteratorKey, out IEnumerator<Vector3> iterator))
            {
                Return(false);
                return;
            }
        
            iterator.MoveNext();
            Return(true);
        }
    }
}