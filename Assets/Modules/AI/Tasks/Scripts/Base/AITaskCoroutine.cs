using System.Collections;
using UnityEngine;

namespace AI.Tasks
{
    public abstract class AITaskCoroutine : AITask
    {
        private Coroutine coroutine;

        protected override void Do()
        {
            coroutine = MonoHelper.Instance.StartCoroutine(DoAsync());            
        }

        protected override void OnCancel()
        {
            if (coroutine != null)
            {
                MonoHelper.Instance.StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        protected abstract IEnumerator DoAsync();
    }
}