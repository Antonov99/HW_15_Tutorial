using AI.Blackboards;
using AI.BTree;
using Entities;

namespace Game.GameEngine.AI
{
    public sealed class BTNode_Entity_FollowEntity : BehaviourNode
    {
        private readonly Agent_Entity_FollowEntity followAgent = new();

        private string unitKey;

        private string targetKey;

        private string stoppingDistanceKey;

        private IBlackboard blackboard;

        public void ConstructBlackboard(IBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public void ConstructKeys(
            string unitKey,
            string targetKey,
            string stoppingDistanceKey 
        )
        {
            this.unitKey = unitKey;
            this.targetKey = targetKey;
            this.stoppingDistanceKey = stoppingDistanceKey;
        }

        protected override void Run()
        {
            if (!blackboard.TryGetVariable(unitKey, out IEntity unit))
            {
                Return(false);
                return;
            }

            if (!blackboard.TryGetVariable(targetKey, out IEntity target))
            {
                Return(false);
                return;
            }

            if (!blackboard.TryGetVariable(stoppingDistanceKey, out float stoppingDistance))
            {
                Return(false);
                return;
            }

            followAgent.OnTargetReached += OnTargetReached;
            followAgent.SetFollowingEntity(unit);
            followAgent.SetTargetEntity(target);
            followAgent.SetStoppingDistance(stoppingDistance);
            followAgent.Play();
        }

        private void OnTargetReached(bool isReached)
        {
            if (isReached)
            {
                Return(true);
            }
        }

        protected override void OnDispose()
        {
            followAgent.OnTargetReached -= OnTargetReached;
            followAgent.Stop();
        }
    }
}