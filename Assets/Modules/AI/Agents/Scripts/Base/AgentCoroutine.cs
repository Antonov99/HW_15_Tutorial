using System.Collections;
using UnityEngine;

namespace AI.Agents
{
    public abstract class AgentCoroutine : Agent
    {
        private YieldInstruction framePeriod;

        private Coroutine coroutine;

        public void SetFramePeriod(YieldInstruction framePeriod)
        {
            this.framePeriod = framePeriod;
        }

        protected override void OnStart()
        {
            coroutine = MonoHelper.Instance.StartCoroutine(LoopCoroutine());
        }

        protected override void OnStop()
        {
            if (coroutine != null)
            {
                MonoHelper.Instance.StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        private IEnumerator LoopCoroutine()
        {
            while (true)
            {
                yield return framePeriod;
                Update();
            }
        }

        protected abstract void Update();
    }
}