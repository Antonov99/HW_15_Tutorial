using System.Collections;
using UnityEngine;

namespace AI.GOAP
{
    [AddComponentMenu("AI/GOAP/Goal Oriented Restarter")]
    [RequireComponent(typeof(GoalOrientedAgent))]
    [DisallowMultipleComponent]
    public class GoalOrientedRestarter : MonoBehaviour
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
                agent.Replay();
            }
        }
    }
}