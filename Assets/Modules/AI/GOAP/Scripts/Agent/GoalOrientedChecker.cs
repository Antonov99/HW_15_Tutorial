using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI.GOAP
{
    [AddComponentMenu("AI/GOAP/Goal Oriented Checker")]
    [RequireComponent(typeof(GoalOrientedAgent))]
    [DisallowMultipleComponent]
    public sealed class GoalOrientedChecker : MonoBehaviour
    {
        [Space]
        [SerializeField]
        private bool playOnStart = true;

        [Space]
        [SerializeField]
        private float minScanPeriod = 0.1f;

        [SerializeField]
        private float maxScanPeriod = 0.2f;

        private GoalOrientedAgent agent;

        private Coroutine coroutine;

        private void Awake()
        {
            agent = GetComponent<GoalOrientedAgent>();
        }

        private void Start()
        {
            if (playOnStart)
            {
                Play();
            }
        }

        public void Play()
        {
            if (coroutine == null)
            {
                coroutine = StartCoroutine(CheckState());
            }
        }

        public void Stop()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        private IEnumerator CheckState()
        {
            while (true)
            {
                var period = Random.Range(minScanPeriod, maxScanPeriod);
                yield return new WaitForSeconds(period);
                SynchronizeGoal();
            }
        }

        private void SynchronizeGoal()
        {
            var actualGoal = agent.Goals
                .Where(it => it.IsValid())
                .OrderByDescending(it => it.EvaluatePriority())
                .FirstOrDefault();

            if (actualGoal == null)
            {
                agent.Cancel();
            }
            else if (!actualGoal.Equals(agent.CurrentGoal))
            {
                agent.Replay();
            }
        }
    }
}