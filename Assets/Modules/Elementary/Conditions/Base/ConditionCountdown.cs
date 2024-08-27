using System.Collections;
using UnityEngine;

namespace Elementary
{
    public sealed class ConditionCountdown : ICondition
    {
        private readonly MonoBehaviour coroutineDispatcher;
        
        private float remainingSeconds;

        private Coroutine coroutine;

        public ConditionCountdown(MonoBehaviour coroutineDispatcher, float seconds, bool startInstantly)
        {
            remainingSeconds = seconds;
            this.coroutineDispatcher = coroutineDispatcher;
            
            if (startInstantly)
            {
                StartCountdown();
            }
        }

        public bool IsTrue()
        {
            return remainingSeconds <= 0.0f;
        }

        public void StartCountdown()
        {
            if (coroutine == null)
            {
                coroutine = coroutineDispatcher.StartCoroutine(CountdownRoutine());
            }
        }

        private IEnumerator CountdownRoutine()
        {
            while (remainingSeconds > 0.0f)
            {
                remainingSeconds -= Time.deltaTime;
                yield return null;
            }
        }
    }
}