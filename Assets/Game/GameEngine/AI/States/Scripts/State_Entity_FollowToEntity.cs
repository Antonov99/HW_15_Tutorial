using System;
using AI.Blackboards;
using Elementary;
using Entities;
using Game.GameEngine.Mechanics;
using Declarative;
using UnityEngine;

namespace Game.GameEngine.AI
{
    [Serializable]
    public sealed class State_Entity_FollowToEntity : State, IFixedUpdateListener
    {
        public string UnitKey
        {
            set => unitKey = value;
        }

        public string TargetKey
        {
            set => targetKey = value;
        }

        public string StoppingDistanceKey
        {
            set => stoppingDistanceKey = value;
        }

        private Agent_Entity_MoveToPosition moveAgent = new();

        private IBlackboard blackboard;

        [BlackboardKey]
        [SerializeField]
        private string unitKey;

        [BlackboardKey]
        [SerializeField]
        private string targetKey;

        [BlackboardKey]
        [SerializeField]
        private string stoppingDistanceKey;

        private IComponent_GetPosition targetPositionComponent;

        private bool enabled;

        public void Construct(IBlackboard blackboard, float stoppingDistance)
        {
            this.blackboard = blackboard;
            moveAgent.SetStoppingDistance(stoppingDistance);
        }

        public override void Enter()
        {
            if (!blackboard.TryGetVariable(unitKey, out IEntity unit))
            {
                return;
            }

            if (!blackboard.TryGetVariable(targetKey, out IEntity target))
            {
                return;
            }

            if (!blackboard.TryGetVariable(stoppingDistanceKey, out float stoppingDistance))
            {
                return;
            }

            targetPositionComponent = target.Get<IComponent_GetPosition>();

            moveAgent.SetMovingEntity(unit); //Unit
            moveAgent.SetStoppingDistance(stoppingDistance); //Stopping Distance
            moveAgent.SetTarget(targetPositionComponent.Position);
            moveAgent.Play();
            
            enabled = true;
        }

        public override void Exit()
        {
            enabled = false;
            moveAgent.Stop();
        }

        void IFixedUpdateListener.FixedUpdate(float deltaTime)
        {
            if (enabled)
            {
                var nextPosition = targetPositionComponent.Position;
                moveAgent.SetTarget(nextPosition);
            }
        }
    }
}