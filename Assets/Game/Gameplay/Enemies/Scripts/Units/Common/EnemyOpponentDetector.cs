using AI.Blackboards;
using Entities;
using Game.GameEngine.Mechanics;

namespace Game.Gameplay.Enemies
{
    public sealed class EnemyOpponentDetector : ColliderDetectionHandler_TargetEntity
    {
        private readonly IBlackboard blackboard;

        private readonly string targetKey;

        public EnemyOpponentDetector(
            IBlackboard blackboard,
            string targetKey,
            ScriptableEntityCondition[] detectConditions
        )
        {
            this.blackboard = blackboard;
            this.targetKey = targetKey;
            conditions = detectConditions;
        }

        protected override void ProcessTarget(bool targetFound, IEntity target)
        {
            if (targetFound && !blackboard.HasVariable(targetKey))
            {
                blackboard.AddVariable(targetKey, target);
                return;
            }

            if (!targetFound && blackboard.HasVariable(targetKey))
            {
                blackboard.RemoveVariable(targetKey);
            }
        }
    }
}