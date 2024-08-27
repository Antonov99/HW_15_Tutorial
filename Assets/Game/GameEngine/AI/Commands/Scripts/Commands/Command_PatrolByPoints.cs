using AI.Blackboards;
using UnityEngine;

namespace Game.GameEngine.AI
{
    public sealed class Command_PatrolByPoints : Command_BTNode<CommandArgs_PatrolByPoints>
    {
        public string PatrolIteratorKey
        {
            set { patrolIteratorKey = value; }
        }

        [Space]
        [BlackboardKey]
        [SerializeField]
        private string patrolIteratorKey;

        private IBlackboard blackboard;

        public void Construct(IBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        protected override void Execute(CommandArgs_PatrolByPoints args)
        {
            var patrolIterator = args.CreateIterator();
            blackboard.ReplaceVariable(patrolIteratorKey, patrolIterator);
            base.Execute(args);
        }

        protected override void OnInterrupt()
        {
            blackboard.RemoveVariable(patrolIteratorKey);
            base.OnInterrupt();
        }
    }
}