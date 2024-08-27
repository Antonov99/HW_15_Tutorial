using AI.BTree;
using AI.Commands;

namespace Game.GameEngine.AI
{
    public class Command_BTNode<T> : AICommand<T>, IBehaviourCallback
    {
        public IBehaviourNode BehaviourNode
        {
            set { behaviourNode = value; }
        }

        private IBehaviourNode behaviourNode;

        public Command_BTNode()
        {
        }

        public Command_BTNode(IBehaviourNode node)
        {
            behaviourNode = node;
        }

        protected override void Execute(T args)
        {
            behaviourNode.Run(callback: this);
        }

        protected override void OnInterrupt()
        {
            behaviourNode.Abort();
        }

        void IBehaviourCallback.Invoke(IBehaviourNode node, bool success)
        {
            Return(success);
        }
    }
}