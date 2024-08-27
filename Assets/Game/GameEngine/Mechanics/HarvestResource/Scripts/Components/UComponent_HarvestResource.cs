using System;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Harvest Resource/Component «Harvest Resource»")]
    public sealed class UComponent_HarvestResource : MonoBehaviour, IComponent_HarvestResource
    {
        public event Action<HarvestResourceOperation> OnHarvestStarted
        {
            add { harvestEngine.OnStarted += value; }
            remove { harvestEngine.OnStarted -= value; }
        }

        public event Action<HarvestResourceOperation> OnHarvestStopped
        {
            add { harvestEngine.OnStopped += value; }
            remove { harvestEngine.OnStopped -= value; }
        }

        public bool IsHarvesting
        {
            get { return harvestEngine.IsActive; }
        }

        [SerializeField]
        private UHarvestResourceOperator harvestEngine;

        public bool CanStartHarvest(HarvestResourceOperation operation)
        {
            return harvestEngine.CanStart(operation);
        }

        public void StartHarvest(HarvestResourceOperation operation)
        {
            harvestEngine.DoStart(operation);
        }

        public void StopHarvest()
        {
            harvestEngine.Stop();
        }
    }
}