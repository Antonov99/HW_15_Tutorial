using System.Collections;
using UnityEngine;

namespace AI.BTree
{
    public abstract class UnityBehaviourNode_Coroutine : UnityBehaviourNode
    {
        private Coroutine coroutine;

        protected sealed override void Run()
        {
            coroutine = StartCoroutine(RunRoutine());
        }

        protected abstract IEnumerator RunRoutine();

        protected override void OnDispose()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }
    }
}