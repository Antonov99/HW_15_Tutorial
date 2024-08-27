using Elementary;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    public abstract class State_CheckDistanceToTarget : StateFixedUpdate
    {
        private ITransformEngine transform;

        private IValue<float> minDistance;

        public void ConstructTransform(ITransformEngine transform)
        {
            this.transform = transform;
        }

        public void ConstructMinDistance(IValue<float> minDistance)
        {
            this.minDistance = minDistance;
        }

        protected override void FixedUpdate(float deltaTime)
        {
            var targetPosiiton = GetTargetPosition();
            var distanceReached = transform.IsDistanceReached(targetPosiiton, minDistance.Current);
            ProcessDistance(distanceReached);
        }

        protected abstract void ProcessDistance(bool distanceReached);
        
        protected abstract Vector3 GetTargetPosition();
    }
}