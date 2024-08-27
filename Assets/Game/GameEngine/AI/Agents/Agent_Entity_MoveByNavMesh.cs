using AI.Agents;
using Entities;
using Game.GameEngine.Mechanics;
using UnityEngine;

namespace Game.GameEngine.AI
{
    public sealed class Agent_Entity_MoveByNavMesh : Agent_MoveByNavMesh
    {
        protected override Agent_MoveByPoints<Vector3> MoveAgent
        {
            get { return moveAgent; }
        }

        private readonly Agent_Entity_MoveByPoints moveAgent = new();

        private IComponent_GetPosition positionComponent;

        public void SetMovingEntity(IEntity movingEntity)
        {
            positionComponent = movingEntity.Get<IComponent_GetPosition>();
            moveAgent.SetMovingEntity(movingEntity);
        }

        public void SetStoppingDistance(float stoppingDistance)
        {
            moveAgent.SetStoppingDistance(stoppingDistance);
        }

        protected override Vector3 EvaluateCurrentPosition()
        {
            return positionComponent.Position;
        }
    }
}