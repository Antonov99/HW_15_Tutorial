using System;
using Sirenix.OdinInspector;

namespace Game.Meta
{
    public abstract class Upgrade
    {
        public event Action<int> OnLevelUp;

        [ReadOnly]
        [ShowInInspector]
        public string Id
        {
            get { return config.id; }
        }

        [ReadOnly]
        [ShowInInspector]
        public int Level
        {
            get { return currentLevel; }
        }

        [ReadOnly]
        [ShowInInspector]
        public int MaxLevel
        {
            get { return config.maxLevel; }
        }

        public bool IsMaxLevel
        {
            get { return currentLevel == config.maxLevel; }
        }

        public float Progress
        {
            get { return (float) currentLevel / config.maxLevel; }
        }

        [ReadOnly]
        [ShowInInspector]
        public UpgradeMetadata Metadata
        {
            get { return config.metadata; }
        }

        [ReadOnly]
        [ShowInInspector]
        public abstract string CurrentStats { get; }

        [ReadOnly]
        [ShowInInspector]
        public abstract string NextImprovement { get; }

        [ReadOnly]
        [ShowInInspector]
        public int NextPrice
        {
            get { return config.priceTable.GetPrice(Level + 1); }
        }

        private readonly UpgradeConfig config;

        private int currentLevel;

        protected Upgrade(UpgradeConfig config)
        {
            this.config = config;
            currentLevel = 1;
        }

        public void SetupLevel(int level)
        {
            currentLevel = level;
        }

        public void LevelUp()
        {
            if (Level >= MaxLevel)
            {
                throw new Exception($"Can not increment level for upgrade {config.id}!");
            }

            var nextLevel = Level + 1;
            currentLevel = nextLevel;
            LevelUp(nextLevel);
            OnLevelUp?.Invoke(nextLevel);
        }

        protected abstract void LevelUp(int level);
    }
}