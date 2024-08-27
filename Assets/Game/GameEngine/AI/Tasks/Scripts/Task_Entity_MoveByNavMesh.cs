using AI.Tasks;
using Entities;
using Game.GameEngine.Mechanics;
using UnityEngine;

namespace Game.GameEngine.AI
{
    public sealed class Task_Entity_MoveByNavMesh : AITask_MoveByNavMesh
    {
        protected override AITask_MoveByPoints<Vector3> MoveTask
        {
            get { return moveTask; }
        }

        private readonly Task_Entity_MoveByPoints moveTask;

        private IComponent_GetPosition positionComponent;

        public Task_Entity_MoveByNavMesh(MonoBehaviour coroutineDispatcher)
        {
            moveTask = new Task_Entity_MoveByPoints();
        }

        public void SetMovingEntity(IEntity movingEntity)
        {
            positionComponent = movingEntity.Get<IComponent_GetPosition>();
            moveTask.SetMovingEntity(movingEntity);
        }

        public void SetStoppingDistance(float stoppingDistance)
        {
            moveTask.SetStoppingDistance(stoppingDistance);
        }

        protected override Vector3 EvaluateStartPosition()
        {
            return positionComponent.Position;
        }
    }
}