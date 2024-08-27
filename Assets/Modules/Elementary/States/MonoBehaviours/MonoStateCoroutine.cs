using System.Collections;
using UnityEngine;

namespace Elementary
{
    public abstract class MonoStateCoroutine : MonoState
    {
        private Coroutine coroutine;
        
        public override void Enter()
        {
            if (coroutine == null)
            {
                coroutine = StartCoroutine(Do());
            }
        }

        public override void Exit()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        protected abstract IEnumerator Do();
    }
}