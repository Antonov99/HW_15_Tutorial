using AI.Blackboards;
using AI.BTree;
using Entities;
using Game.GameEngine.Mechanics;
using UnityEngine;

namespace Game.GameEngine.AI
{
    [AddComponentMenu(BehaviourTreePaths.MENU_PATH + "BTNode «Melee Combat» (Entity)")]
    public sealed class UBTNode_Entity_MeleeCombat : UnityBehaviourNode, IBlackboardInjective
    {
        public IBlackboard Blackboard { private get; set; }

        [Space]
        [BlackboardKey]
        [SerializeField]
        private string unitKey;
        
        [BlackboardKey]
        [SerializeField]
        private string entityKey;

        private IComponent_MeleeCombat unitComponent;
        
        protected override void Run()
        {
            if (!Blackboard.TryGetVariable(unitKey, out IEntity unit))
            {
                Return(false);
                return;   
            }
            
            if (!Blackboard.TryGetVariable(entityKey, out IEntity target))
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