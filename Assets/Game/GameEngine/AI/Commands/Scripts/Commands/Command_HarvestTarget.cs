using System;
using AI.Blackboards;
using UnityEngine;

namespace Game.GameEngine.AI
{
    [Serializable]
    public sealed class Command_HarvestTarget : Command_BTNode<CommandArgs_HarvestTarget>
    {
        public string TargetKey
        {
            set { targetKey = value; }
        }

        [Space]
        [BlackboardKey]
        [SerializeField]
        private string targetKey;

        private IBlackboard blackboard;

        public void Construct(IBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        protected override void Execute(CommandArgs_HarvestTarget args)
        {
            blackboard.ReplaceVariable(targetKey, args.target);
            base.Execute(args);
        }

        protected override void OnInterrupt()
        {
            blackboard.RemoveVariable(targetKey);
            base.OnInterrupt();
        }
    }
}