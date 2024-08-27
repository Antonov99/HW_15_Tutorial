using UnityEngine;

namespace AI.BTree
{
    [AddComponentMenu(Extensions.MENU_PATH + "Decorator Node")]
    public sealed class UnityBehaviourNode_Decorator : UnityBehaviourNode, IBehaviourCallback
    {
        [SerializeField]
        private UnityBehaviourNode node;

        [SerializeField]
        private bool success = true;

        protected override void Run()
        {
            node.Run(callback: this);
        }

        void IBehaviourCallback.Invoke(IBehaviourNode node, bool success)
        {
            Return(this.success);
        }

        protected override void OnAbort()
        {
            if (node.IsRunning)
            {
                node.Abort();
            }
        }
    }
}