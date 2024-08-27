using UnityEngine;

namespace AI.BTree
{
    [AddComponentMenu(Extensions.MENU_PATH + "Node «Do And Inspect»")]
    public sealed class UnityBehaviourNode_DoAndInspect : UnityBehaviourNode, IBehaviourCallback
    {
        [SerializeField]
        private UnityBehaviourNode actionNode;

        [SerializeField]
        private UnityBehaviourNode[] inspectorNodes;

        protected override void Run()
        {
            actionNode.Run(callback: this);
            
            for (int i = 0, count = inspectorNodes.Length; i < count; i++)
            {
                var inspector = inspectorNodes[i];
                inspector.Run(callback: this);
            }
        }

        void IBehaviourCallback.Invoke(IBehaviourNode node, bool success)
        {
            if (ReferenceEquals(node, actionNode))
            {
                Return(success);
            }
            else //Any inspector node
            {
                Return(false);
            }
        }

        protected override void OnAbort()
        {
            if (actionNode.IsRunning)
            {
                actionNode.Abort();
            }
            
            for (int i = 0, count = inspectorNodes.Length; i < count; i++)
            {
                var inspector = inspectorNodes[i];
                if (inspector.IsRunning)
                {
                    inspector.Abort();
                }
            }
        }
    }
}