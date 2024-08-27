using System.Collections.Generic;
using AI.Blackboards;
using AI.BTree;
using UnityEngine;

namespace Game.GameEngine.AI
{
    [AddComponentMenu(BehaviourTreePaths.MENU_PATH + "BTNode «Move Next» (Iterator)")]
    public sealed class UBTNode_Iterator_MoveNext : UnityBehaviourNode, IBlackboardInjective
    {
        public IBlackboard Blackboard { private get; set; }

        [BlackboardKey]
        [SerializeField]
        private string iteratorKey;

        protected override void Run()
        {
            if (!Blackboard.TryGetVariable(iteratorKey, out IEnumerator<Vector3> iterator))
            {
                Return(false);
                return;
            }

            iterator.MoveNext();
            Return(true);
        }
    }
}