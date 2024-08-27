using UnityEngine;

namespace AI.GOAP
{
    public abstract class Goal : MonoBehaviour, IGoal
    {
        public IFactState ResultState
        {
            get { return resultState; }
        }

        [SerializeField]
        private FactState resultState;

        public abstract bool IsValid();

        public abstract int EvaluatePriority();
    }
}