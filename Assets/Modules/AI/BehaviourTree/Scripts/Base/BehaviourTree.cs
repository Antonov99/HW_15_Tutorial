using System;
using UnityEngine;

namespace AI.BTree
{
    [Serializable]
    public class BehaviourTree : BehaviourNode, IBehaviourTree, IBehaviourCallback
    {
        public event Action OnStarted;
        
        public event Action<bool> OnFinished;

        public event Action OnAborted;

        [SerializeReference]
        public IBehaviourNode root = default;

        public BehaviourTree(IBehaviourNode root)
        {
            this.root = root;
        }

        public BehaviourTree()
        {
        }

        protected override void Run()
        {
            if (!root.IsRunning)
            {
                OnStarted?.Invoke();
                root.Run(callback: this);
            }
        }

        protected override void OnAbort()
        {
            if (root.IsRunning)
            {
                root.Abort();
                OnAborted?.Invoke();
            }
        }

        void IBehaviourCallback.Invoke(IBehaviourNode node, bool success)
        {
            Return(success);
            OnFinished?.Invoke(success);
        }
    }
}