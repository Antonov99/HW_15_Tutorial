using GameSystem;
using UnityEngine;

namespace Game.Meta
{
    public sealed class MissionAnalyticsTracker : 
        IGameReadyElement,
        IGameFinishElement
    {
        [GameInject]
        private MissionsManager missionsManager;
        
        void IGameReadyElement.ReadyGame()
        {
            missionsManager.OnRewardReceived += OnReceiveRewardQuest;
        }

        void IGameFinishElement.FinishGame()
        {
            missionsManager.OnRewardReceived -= OnReceiveRewardQuest;
        }

        private void OnReceiveRewardQuest(Mission mission)
        {
            MissionAnalytics.LogMissionRewardReceived(mission);
        }
    }
}