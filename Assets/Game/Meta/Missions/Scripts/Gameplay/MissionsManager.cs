using System;
using System.Collections.Generic;
using System.Linq;
using Game.Gameplay.Player;
using GameSystem;
using Sirenix.OdinInspector;

namespace Game.Meta
{
    public sealed class MissionsManager : 
        IGameStartElement,
        IGameFinishElement
    {
        public event Action<Mission> OnRewardReceived;
        
        public event Action<Mission> OnMissionChanged;

        private MissionFactory factory;

        private MissionSelector selector;
        
        private MoneyStorage moneyStorage;
        
        [ReadOnly, ShowInInspector]
        private readonly Dictionary<MissionDifficulty, Mission> missions = new();

        [GameInject]
        public void Construct(MissionFactory factory, MissionSelector selector, MoneyStorage moneyStorage)
        {
            this.factory = factory;
            this.selector = selector;
            this.moneyStorage = moneyStorage;
        }

        public bool CanReceiveReward(Mission mission)
        {
            return mission.State == MissionState.COMPLETED &&
                   missions.ContainsValue(mission);
        }

        public void ReceiveReward(Mission mission)
        {
            if (!CanReceiveReward(mission))
            {
                throw new Exception($"Can not receive reward from mission {mission.Id}!");
            }

            moneyStorage.EarnMoney(mission.MoneyReward);
            OnRewardReceived?.Invoke(mission);

            GenerateNextMission(mission.Difficulty, mission.Id);
        }

        public Mission GetMission(MissionDifficulty difficulty)
        {
            if (missions.TryGetValue(difficulty, out var mission))
            {
                return mission;
            }

            throw new Exception($"Mission {difficulty} is absent!");
        }

        public Mission[] GetMissions()
        {
            return missions.Values.ToArray();
        }

        public bool IsMissionExists(MissionDifficulty difficulty)
        {
            return missions.ContainsKey(difficulty);
        }

        public Mission SetupMission(MissionConfig missionConfig)
        {
            var mission = factory.CreateMission(missionConfig);
            missions[missionConfig.Difficulty] = mission;
            return mission;
        }
        
        void IGameStartElement.StartGame()
        {
            StartMissions();
        }

        void IGameFinishElement.FinishGame()
        {
            StopMissions();
        }

        private void StartMissions()
        {
            foreach (var mission in missions.Values)
            {
                mission.Start();
            }
        }

        private void StopMissions()
        {
            foreach (var mission in missions.Values)
            {
                mission.Stop();
            }
        }

        private void GenerateNextMission(MissionDifficulty difficulty, string prevMissionId)
        {
            var missionConfig = selector.SelectNextMission(difficulty, prevMissionId);
            var mission = factory.CreateMission(missionConfig);
            missions[difficulty] = mission;

            mission.Start();
            OnMissionChanged?.Invoke(mission);
        }
    }
}