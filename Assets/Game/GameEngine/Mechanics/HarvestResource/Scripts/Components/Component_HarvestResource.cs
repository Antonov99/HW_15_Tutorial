using System;
using Elementary;

namespace Game.GameEngine.Mechanics
{
    public sealed class Component_HarvestResource : IComponent_HarvestResource
    {
        public event Action<HarvestResourceOperation> OnHarvestStarted
        {
            add { harvestOperator.OnStarted += value; }
            remove { harvestOperator.OnStarted -= value; }
        }

        public event Action<HarvestResourceOperation> OnHarvestStopped
        {
            add { harvestOperator.OnStopped += value; }
            remove { harvestOperator.OnStopped -= value; }
        }

        public bool IsHarvesting
        {
            get { return harvestOperator.IsActive; }
        }

        private readonly IOperator<HarvestResourceOperation> harvestOperator;

        public Component_HarvestResource(IOperator<HarvestResourceOperation> harvestOperator)
        {
            this.harvestOperator = harvestOperator;
        }

        public bool CanStartHarvest(HarvestResourceOperation operation)
        {
            return harvestOperator.CanStart(operation);
        }

        public void StartHarvest(HarvestResourceOperation operation)
        {
            harvestOperator.DoStart(operation);
        }

        public void StopHarvest()
        {
            harvestOperator.Stop();
        }
    }
}