using System;
using Sirenix.OdinInspector;

namespace Game.Meta
{
    [Serializable]
    public abstract class Mission
    {
        public event Action<Mission> OnStarted;

        public abstract event Action<Mission> OnProgressChanged;

        public event Action<Mission> OnCompleted;

        [ReadOnly]
        [ShowInInspector]
        public string Id
        {
            get { return config.Id; }
        }

        [ReadOnly]
        [ShowInInspector]
        public MissionState State { get; private set; }

        [ReadOnly]
        [ShowInInspector]
        public MissionDifficulty Difficulty
        {
            get { return config.Difficulty; }
        }

        [ReadOnly]
        [ShowInInspector]
        public int MoneyReward
        {
            get { return config.MoneyReward; }
        }

        [ReadOnly]
        [ShowInInspector]
        public MissionMetadata Metadata
        {
            get { return config.Metadata; }
        }

        [ReadOnly]
        [ShowInInspector]
        public abstract float NormalizedProgress { get; }

        [ReadOnly]
        [ShowInInspector]
        public abstract string TextProgress { get; }

        private readonly MissionConfig config;

        public Mission(MissionConfig config)
        {
            this.config = config;
            State = MissionState.NOT_STARTED;
        }

        public void Start()
        {
            if (State == MissionState.STARTED)
            {
                throw new Exception("Mission is already started!");
            }

            State = MissionState.STARTED;
            OnStarted?.Invoke(this);

            if (NormalizedProgress >= 1.0f)
            {
                Complete();
                return;
            }

            OnStart();
        }

        public void Stop()
        {
            if (State != MissionState.STARTED)
            {
                return;
            }

            State = MissionState.NOT_STARTED;
            OnStop();
        }

        #region Callbacks

        protected abstract void OnStart();

        protected abstract void OnStop();

        #endregion

        protected void TryComplete()
        {
            if (NormalizedProgress >= 1.0f)
            {
                Complete();
            }
        }

        private void Complete()
        {
            if (State != MissionState.STARTED)
            {
                throw new Exception("Mission is not started!");
            }

            State = MissionState.COMPLETED;
            OnStop();
            OnCompleted?.Invoke(this);
        }
    }
}