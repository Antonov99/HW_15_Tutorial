using System;
using UnityEngine;

namespace AI.BTree
{
    [Serializable]
    public class BehaviourNodeSelector : BehaviourNode, IBehaviourCallback
    {
        [SerializeReference]
        public IBehaviourNode[] children;

        private IBehaviourNode currentNode;

        private int pointer;

        public BehaviourNodeSelector(params IBehaviourNode[] children)
        {
            this.children = children;
        }

        public BehaviourNodeSelector()
        {
        }

        protected override void Run()
        {
            if (children == null && children.Length <= 0)
            {
                Return(false);
                return;
            }

            pointer = 0;

            currentNode = children[pointer];
            currentNode.Run(callback: this);
        }

        void IBehaviourCallback.Invoke(IBehaviourNode node, bool success)
        {
            if (success)
            {
                Return(true);
                return;
            }

            if (pointer + 1 >= children.Length)
            {
                Return(false);
                return;
            }

            pointer++;
            currentNode = children[pointer];
            currentNode.Run(callback: this);
        }

        protected override void OnAbort()
        {
            if (currentNode is {IsRunning: true})
            {
                currentNode.Abort();
            }
        }
    }
}