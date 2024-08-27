using Elementary;
using Game.GameEngine.GameResources;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Harvest Resource/Harvest Resource State «Control Resource Capacity»")]
    public sealed class UHarvestResourceState_ControlResourceCapacity : MonoState
    {
        [SerializeField]
        public UResourceSourceLimited resourceStorage;

        [SerializeField]
        public UHarvestResourceOperator harvestEngine;
        
        public override void Enter()
        {
            resourceStorage.OnValueChanged += OnResourceCountChanged;
        }

        public override void Exit()
        {
            resourceStorage.OnValueChanged -= OnResourceCountChanged;
        }

        private void OnResourceCountChanged(ResourceType resourceType, int newCount)
        {
            if (resourceStorage.IsLimit)
            {
                harvestEngine.Stop();
            }
        }
    }
}