using System;
using Elementary;
using Game.GameEngine.GameResources;
using Game.Gameplay.Conveyors;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class Component_UnloadZone : IComponent_UnloadZone
    {
        public event Action<int> OnAmountChanged
        {
            add { storage.OnValueChanged += value; }
            remove { storage.OnValueChanged -= value; }
        }

        public int MaxAmount
        {
            get { return storage.MaxValue; }
        }

        public int CurrentAmount
        {
            get { return storage.Current; }
        }

        public bool IsFull
        {
            get { return storage.IsLimit; }
        }

        public bool IsEmpty
        {
            get { return storage.Current <= 0; }
        }

        public ResourceType ResourceType
        {
            get { return resourceType; }
        }

        public Vector3 ParticlePosition
        {
            get { return particlePoint.position; }
        }

        private readonly IVariableLimited<int> storage;

        private readonly ResourceType resourceType;

        private readonly Transform particlePoint;

        public Component_UnloadZone(IVariableLimited<int> storage, ResourceType resourceType, Transform particlePoint)
        {
            this.storage = storage;
            this.resourceType = resourceType;
            this.particlePoint = particlePoint;
        }

        public void SetupAmount(int currentAmount)
        {
            storage.Current = currentAmount;
        }

        public int ExtractAll()
        {
            var resources = storage.Current;
            storage.Current = 0;
            return resources;
        }
    }
}