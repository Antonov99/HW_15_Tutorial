using System.Collections;
using UnityEngine;

namespace AI.BTree
{
    [AddComponentMenu(Extensions.MENU_PATH + "Node «Endless Loop»")]
    public sealed class UnityBehaviourNode_EndlessLoop : UnityBehaviourNode, IBehaviourCallback
    {
        [SerializeField]
        private UnityBehaviourNode child;

        protected override void Run()
        {
            child.Run(callback: this);
        }

        void IBehaviourCallback.Invoke(IBehaviourNode node, bool success)
        {
            StartCoroutine(RunInNextFrame());
        }

        private IEnumerator RunInNextFrame()
        {
            yield return new WaitForEndOfFrame();
            child.Run(callback: this);
        }

        protected override void OnAbort()
        {
            StopAllCoroutines();
            if (child.IsRunning)
            {
                child.Abort();
            }
        }
    }
}