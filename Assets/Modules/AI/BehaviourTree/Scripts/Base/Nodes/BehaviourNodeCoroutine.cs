using System.Collections;
using UnityEngine;

namespace AI.BTree
{
    public abstract class BehaviourNodeCoroutine : BehaviourNode
    {
        private Coroutine coroutine;

        protected sealed override void Run()
        {
            coroutine = MonoHelper.Instance.StartCoroutine(RunRoutine());
        }

        protected abstract IEnumerator RunRoutine();

        protected override void OnDispose()
        {
            if (coroutine != null)
            {
                MonoHelper.Instance.StopCoroutine(coroutine);
                coroutine = null;
            }
        }
    }
}