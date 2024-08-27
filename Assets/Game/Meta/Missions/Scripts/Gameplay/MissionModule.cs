using GameSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Meta
{
    public sealed class MissionModule : GameModule, IGameInitElement
    {
        [GameService]
        [SerializeField]
        private MissionCatalog catalog;
        
        [GameService, GameElement]
        [Space, ReadOnly, ShowInInspector]
        private MissionsManager manager = new();

        [GameElement]
        [ReadOnly, ShowInInspector]
        private MissionFactory factory = new();

        [ReadOnly, ShowInInspector]
        private MissionSelector selector = new();

        [GameElement]
        [ReadOnly, ShowInInspector]
        private MissionAnalyticsTracker analyticsTracker = new();

        [Title("Initial missions")]
        [SerializeField]
        private MissionConfig easyMission;

        [SerializeField]
        private MissionConfig normalMission;

        [SerializeField]
        private MissionConfig hardMission;
        
        void IGameInitElement.InitGame()
        {
            if (!manager.IsMissionExists(MissionDifficulty.EASY))
            {
                manager.SetupMission(easyMission);
            }

            if (!manager.IsMissionExists(MissionDifficulty.NORMAL))
            {
                manager.SetupMission(normalMission);
            }

            if (!manager.IsMissionExists(MissionDifficulty.HARD))
            {
                manager.SetupMission(hardMission);
            }
        }
    }
}