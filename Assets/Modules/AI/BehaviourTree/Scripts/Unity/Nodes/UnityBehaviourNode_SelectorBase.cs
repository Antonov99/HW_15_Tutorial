namespace AI.BTree
{
    public abstract class UnityBehaviourNode_SelectorBase : UnityBehaviourNode, IBehaviourCallback
    {
        protected abstract UnityBehaviourNode[] Children { get; }
        
        private int pointer;

        private UnityBehaviourNode currentNode;

        protected override void Run()
        {
            var children = Children;
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

            var children = Children;
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
            if (currentNode != null && currentNode.IsRunning)
            {
                currentNode.Abort();
            }
        }
    }
}