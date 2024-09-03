using System;
using Game.Meta;

namespace Game.Tutorial
{
    public sealed class GetRewardInspector
    {
        private MissionsManager missionsManager;
        private RewardConfig config;

        private Mission targetMission;
        
        private Action callback;
        
        public void Construct(MissionsManager missionsManager, RewardConfig config)
        {
            this.missionsManager = missionsManager;
            this.config = config;
        }
        
        public void Inspect(Action callback)
        {
            this.callback = callback;
            targetMission = missionsManager.GetMission(config.missionConfig.Difficulty);
            missionsManager.OnRewardReceived += OnRewardReceived;
        }
        
        private void OnRewardReceived(Mission mission)
        {
            if (mission != targetMission) return;
            missionsManager.OnRewardReceived -= OnRewardReceived;
            callback?.Invoke();
        }
    }
}