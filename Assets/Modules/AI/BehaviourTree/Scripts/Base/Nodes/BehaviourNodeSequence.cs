using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI.BTree
{
    [Serializable]
    public class BehaviourNodeSequence : BehaviourNode, IBehaviourCallback
    {
        [SerializeReference]
        public IBehaviourNode[] children;

        private IBehaviourNode currentNode;

        private int pointer;

        public BehaviourNodeSequence(params IBehaviourNode[] children)
        {
            this.children = children;
        }

        public BehaviourNodeSequence(IEnumerable<IBehaviourNode> children)
        {
            this.children = children.ToArray();
        }

        public BehaviourNodeSequence()
        {
        }

        protected override void Run()
        {
            if (children.Length <= 0)
            {
                Return(true);
                return;
            }

            pointer = 0;
            currentNode = children[pointer];
            currentNode.Run(callback: this);
        }

        void IBehaviourCallback.Invoke(IBehaviourNode node, bool success)
        {
            if (!success)
            {
                Return(false);
                return;
            }

            if (pointer + 1 >= children.Length)
            {
                Return(true);
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