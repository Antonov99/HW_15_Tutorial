using System;
using AI.Blackboards;
using AI.BTree;
using Entities;
using Game.GameEngine.Mechanics;

namespace Game.GameEngine.AI
{
    [Serializable]
    public sealed class BTNode_Entity_MeleeCombat : BehaviourNode
    {
        private IBlackboard blackboard;

        private string attackerKey;

        private string targetKey;

        private IComponent_MeleeCombat unitComponent;

        public void ConstructBlackboard(IBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public void ConstructBlackboardKeys(string attackerKey, string targetKey)
        {
            this.attackerKey = attackerKey;
            this.targetKey = targetKey;
        }

        protected override void Run()
        {
            if (!blackboard.TryGetVariable(attackerKey, out IEntity unit))
            {
                Return(false);
                return;
            }

            if (!blackboard.TryGetVariable(targetKey, out IEntity target))
            {
                Return(false);
                return;
            }

            unitComponent = unit.Get<IComponent_MeleeCombat>();
            TryStartCombat(target);
        }

        private void TryStartCombat(IEntity target)
        {
            var operation = new CombatOperation(target);
            if (unitComponent.CanStartCombat(operation))
            {
                unitComponent.OnCombatStopped += OnCombatFinished;
                unitComponent.StartCombat(operation);
            }
            else
            {
                Return(false);
            }
        }

        private void OnCombatFinished(CombatOperation operation)
        {
            if (unitComponent != null)
            {
                unitComponent.OnCombatStopped -= OnCombatFinished;
                unitComponent = null;
            }

            var success = operation.targetDestroyed;
            Return(success);
        }

        protected override void OnAbort()
        {
            if (unitComponent != null)
            {
                unitComponent.OnCombatStopped -= OnCombatFinished;
                unitComponent.StopCombat();
                unitComponent = null;
            }
        }
    }
}