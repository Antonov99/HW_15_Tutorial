using AI.Blackboards;
using AI.BTree;
using Entities;
using Polygons;
using UnityEngine;

namespace Game.GameEngine.AI
{
    public sealed class BTNode_Entity_FollowEntityByPolygon : BehaviourNode
    {
        private readonly Agent_Entity_FollowEntityByPolygon followAgent = new();

        private string unitKey;

        private string targetKey;

        private string surfaceKey;

        private IBlackboard blackboard;
        
        public BTNode_Entity_FollowEntityByPolygon()
        {
            followAgent.SetCalculatePathPeriod(new WaitForFixedUpdate());
            followAgent.SetCheckTargetReachedPeriod(null);
        }

        public void ConstructBlackboard(IBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public void ConstructBlackboardKeys(string unitKey, string targetKey, string surfaceKey)
        {
            this.unitKey = unitKey;
            this.targetKey = targetKey;
            this.surfaceKey = surfaceKey;
        }

        public void ConstructStoppingDistance(float distance)
        {
            followAgent.SetStoppingDistance(distance);
        }

        public void ConstructIntermediateDistance(float distance)
        {
            followAgent.SetIntermediateDistance(distance);
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

            if (!blackboard.TryGetVariable(surfaceKey, out MonoPolygon polygon))
            {
                Return(false);
                return;
            }

            followAgent.OnTargetReached += OnTargetReached;
            followAgent.SetSurface(polygon);
            followAgent.SetTargetEntity(target);
            followAgent.SetFollowingEntity(unit);
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
            followAgent.Stop();
            followAgent.OnTargetReached -= OnTargetReached;
        }
    }
}