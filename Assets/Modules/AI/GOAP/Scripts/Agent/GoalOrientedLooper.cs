using UnityEngine;

namespace AI.GOAP
{
    [AddComponentMenu("AI/GOAP/Goal Oriented Looper")]
    [RequireComponent(typeof(GoalOrientedAgent))]
    [DisallowMultipleComponent]
    public sealed class GoalOrientedLooper : MonoBehaviour
    {
        private GoalOrientedAgent agent;

        private void Awake()
        {
            agent = GetComponent<GoalOrientedAgent>();
        }

        private void FixedUpdate()
        {
            if (!agent.IsPlaying)
            {
                agent.Play();
            }
        }
    }
}