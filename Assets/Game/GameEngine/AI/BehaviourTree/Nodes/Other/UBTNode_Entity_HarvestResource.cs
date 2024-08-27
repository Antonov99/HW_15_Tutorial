using AI.Blackboards;
using AI.BTree;
using Entities;
using Game.GameEngine.Mechanics;
using UnityEngine;

namespace Game.GameEngine.AI
{
    [AddComponentMenu(BehaviourTreePaths.MENU_PATH + "BTNode «Harvest Resource» (Entity)")]
    public sealed class UBTNode_Entity_HarvestResource : UnityBehaviourNode, IBlackboardInjective
    {
        public IBlackboard Blackboard { private get; set; }

        [BlackboardKey]
        [SerializeField]
        private string unitKey;

        [BlackboardKey]
        [SerializeField]
        private string resourceKey;

        private IComponent_HarvestResource harvestComponent;

        protected override void Run()
        {
            if (!Blackboard.TryGetVariable(unitKey, out IEntity unit))
            {
                Return(false);
                return;
            }

            if (!Blackboard.TryGetVariable(resourceKey, out IEntity resource))
            {
                Return(false);
                return;
            }

            harvestComponent = unit.Get<IComponent_HarvestResource>();
            harvestComponent.OnHarvestStopped += OnHarvestFinished;
            
            StartHarvest(resource);
        }

        private void StartHarvest(IEntity targetResource)
        {
            var operation = new HarvestResourceOperation(targetResource); 
            harvestComponent.StartHarvest(operation);
        }

        private void OnHarvestFinished(HarvestResourceOperation operation)
        {
            if (operation.isCompleted)
            {
                Return(true);
            }
            else
            {
                Return(false);
            }
        }

        protected override void OnAbort()
        {
            if (harvestComponent != null)
            {
                harvestComponent.OnHarvestStopped -= OnHarvestFinished;
                harvestComponent.StopHarvest();
                harvestComponent = null;   
            }
        }

        protected override void OnReturn(bool success)
        {
            if (harvestComponent != null)
            {
                harvestComponent.OnHarvestStopped -= OnHarvestFinished;
                harvestComponent = null;
            }
        }
    }
}