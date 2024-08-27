using AI.Tasks;
using Entities;
using Game.GameEngine.Mechanics;
using UnityEngine;

namespace Game.GameEngine.AI
{
    public sealed class Task_Entity_MoveByPoints : AITask_MoveByPoints<Vector3>
    {
        private IComponent_MoveInDirection moveComponent;

        private IComponent_GetPosition positionComponent;
        
        private float sqrStoppingDistance = 0.01f;

        public Task_Entity_MoveByPoints() 
        {
            SetFramePeriod(new WaitForFixedUpdate());
        }

        public void SetMovingEntity(IEntity movingEntity)
        {
            moveComponent = movingEntity.Get<IComponent_MoveInDirection>();
            positionComponent = movingEntity.Get<IComponent_GetPosition>();
        }

        public void SetStoppingDistance(float stoppingDistance)
        {
            sqrStoppingDistance = Mathf.Pow(stoppingDistance, 2);
        }
        
        protected override void MoveToPoint(Vector3 target)
        {
            var moveVector = EvaluateMoveVector(target);
            moveComponent.Move(moveVector.normalized);
        }

        protected override bool CheckPointReached(Vector3 target)
        {
            var moveVector = EvaluateMoveVector(target);
            return moveVector.sqrMagnitude <= sqrStoppingDistance;
        }

        private Vector3 EvaluateMoveVector(Vector3 targetPosition)
        {
            var currentPosition = positionComponent.Position;
            var moveVector = targetPosition - currentPosition;
            moveVector.y = 0;
            return moveVector;
        }
    }
}