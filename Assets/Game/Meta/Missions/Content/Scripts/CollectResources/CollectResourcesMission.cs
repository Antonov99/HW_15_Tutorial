using System;
using Entities;
using Game.GameEngine.GameResources;
using Game.GameEngine.Mechanics;
using Game.Gameplay.Hero;
using Game.Gameplay.Player;
using GameSystem;
using Sirenix.OdinInspector;

namespace Game.Meta
{
    public sealed class CollectResourcesMission : Mission
    {
        public override event Action<Mission> OnProgressChanged;

        [ReadOnly]
        [ShowInInspector]
        [PropertySpace(8)]
        public int CurrentResources { get; private set; }

        [ReadOnly]
        [ShowInInspector]
        public int RequiredResources
        {
            get { return config.RequiredResources; }
        }

        [ReadOnly]
        [ShowInInspector]
        public ResourceType ResourceType
        {
            get { return config.ResourceType; }
        }

        public override float NormalizedProgress
        {
            get { return (float) CurrentResources / RequiredResources; }
        }

        public override string TextProgress
        {
            get { return $"{CurrentResources}/{RequiredResources}"; }
        }

        private readonly CollectResourcesMissionConfig config;

        [GameInject]
        private ResourceStorage resourceStorage;

        public CollectResourcesMission(CollectResourcesMissionConfig config) : base(config)
        {
            this.config = config;
            CurrentResources = 0;
        }

        public void Setup(int currentResources)
        {
            CurrentResources = Math.Min(currentResources, RequiredResources);
        }

        protected override void OnStart()
        {
            resourceStorage.OnResourceAdded += OnResourcesAdded;
        }

        protected override void OnStop()
        {
            resourceStorage.OnResourceAdded -= OnResourcesAdded;
        }

        private void OnResourcesAdded(ResourceType resourceType, int income)
        {
            if (resourceType != config.ResourceType)
            {
                return;
            }

            CurrentResources = Math.Min(CurrentResources + income, RequiredResources);
            OnProgressChanged?.Invoke(this);
            TryComplete();
        }
    }
}