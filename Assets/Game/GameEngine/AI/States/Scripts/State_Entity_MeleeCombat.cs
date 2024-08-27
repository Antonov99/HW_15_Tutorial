using System;
using AI.Blackboards;
using Elementary;
using Entities;
using UnityEngine;

namespace Game.GameEngine.AI
{
    [Serializable]
    public sealed class State_Entity_MeleeCombat : State
    {
        public string UnitKey
        {
            set { unitKey = value; }
        }

        public string TargetKey
        {
            set { targetKey = value; }
        }

        private IBlackboard blackboard;

        private Agent_Entity_MeleeCombat meleeAgent = new();
        
        [BlackboardKey]
        [SerializeField]
        private string unitKey;

        [BlackboardKey]
        [SerializeField]
        private string targetKey;

        public void Construct(IBlackboard blackboard)
        {
            this.blackboard = blackboard;
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

            meleeAgent.SetAttacker(unit);
            meleeAgent.SetTarget(target);
            meleeAgent.Play();
        }

        public override void Exit()
        {
            meleeAgent.Stop();
        }
    }
}