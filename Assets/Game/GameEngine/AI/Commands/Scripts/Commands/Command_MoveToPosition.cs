using System;
using AI.Blackboards;
using UnityEngine;

namespace Game.GameEngine.AI
{
    [Serializable]
    public sealed class Command_MoveToPosition : Command_BTNode<CommandArgs_MoveToPosition>
    {
        public string MovePosiitonKey
        {
            set { movePositionKey = value; }
        }

        [Space]
        [BlackboardKey]
        [SerializeField]
        private string movePositionKey;

        private IBlackboard blackboard;

        public void Construct(IBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        protected override void Execute(CommandArgs_MoveToPosition args)
        {
            blackboard.ReplaceVariable(movePositionKey, args.targetPosition);
            base.Execute(args);
        }

        protected override void OnInterrupt()
        {
            blackboard.RemoveVariable(movePositionKey);
            base.OnInterrupt();
        }
    }
}