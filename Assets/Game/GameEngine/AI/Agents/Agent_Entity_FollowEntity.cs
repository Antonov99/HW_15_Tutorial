using System;
using AI.Agents;
using Entities;
using Game.GameEngine.Mechanics;
using UnityEngine;

namespace Game.GameEngine.AI
{
    public class Agent_Entity_FollowEntity : AgentCoroutine
    {
        public event Action<bool> OnTargetReached
        {
            add { moveAgent.OnTargetReached += value; }
            remove { moveAgent.OnTargetReached -= value; }
        }

        public bool IsTargetReached
        {
            get { return moveAgent.IsTargetReached; }
        }

        private readonly Agent_Entity_MoveToPosition moveAgent = new();

        private IComponent_GetPosition targetComponent;

        public Agent_Entity_FollowEntity()
        {
            SetFramePeriod(new WaitForFixedUpdate());
        }

        public void SetStoppingDistance(float stoppingDistance)
        {
            moveAgent.SetStoppingDistance(stoppingDistance);
        }

        public void SetFollowingEntity(IEntity followingEntity)
        {
            moveAgent.SetMovingEntity(followingEntity);
        }

        public void SetTargetEntity(IEntity targetEntity)
        {
            targetComponent = targetEntity.Get<IComponent_GetPosition>();

            var targetPosition = targetComponent.Position;
            moveAgent.SetTarget(targetPosition);
        }

        protected override void OnStart()
        {
            base.OnStart();
            moveAgent.Play();
        }

        protected override void OnStop()
        {
            base.OnStop();
            moveAgent.Stop();
        }

        protected override void Update()
        {
            var targetPosition = targetComponent.Position;
            moveAgent.SetTarget(targetPosition);
        }
    }
}