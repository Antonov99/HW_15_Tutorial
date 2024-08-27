using System;
using Game.Gameplay.Player;
using GameSystem;
using Windows;
using UnityEngine;

namespace Game.Meta
{
    public sealed class MissionListPresenter : MonoWindow, IGameConstructElement
    {
        [SerializeField]
        private MissionItem[] missionItems;

        private MissionsManager missionsManager;

        protected override void OnShow(object args)
        {
            missionsManager.OnMissionChanged += OnMissionChanged;

            var missions = missionsManager.GetMissions();
            for (int i = 0, count = missions.Length; i < count; i++)
            {
                var mission = missions[i];
                var presenter = GetPresenter(mission.Difficulty);
                presenter.Start(mission);
            }
        }

        protected override void OnHide()
        {
            missionsManager.OnMissionChanged -= OnMissionChanged;

            for (int i = 0, count = missionItems.Length; i < count; i++)
            {
                var presenter = missionItems[i].presenter;
                presenter.Stop();
            }
        }
        
        private void OnMissionChanged(Mission mission)
        {
            var presenter = GetPresenter(mission.Difficulty);
            if (presenter.IsShown)
            {
                presenter.Stop();
            }

            presenter.Start(mission);
        }

        void IGameConstructElement.ConstructGame(GameContext context)
        {
            missionsManager = context.GetService<MissionsManager>();
            var moneyPanelAnimator = context.GetService<MoneyPanelAnimator_AddMoney>();

            for (int i = 0, count = missionItems.Length; i < count; i++)
            {
                var missionItem = missionItems[i];
                missionItem.presenter.Construct(missionsManager, moneyPanelAnimator);
            }
        }

        private MissionPresenter GetPresenter(MissionDifficulty difficulty)
        {
            for (int i = 0, count = missionItems.Length; i < count; i++)
            {
                var missionItem = missionItems[i];
                if (missionItem.difficulty == difficulty)
                {
                    return missionItem.presenter;
                }
            }

            throw new Exception($"Mission with difficulty {difficulty} is not found"!);
        }

        [Serializable]
        private sealed class MissionItem
        {
            [SerializeField]
            public MissionDifficulty difficulty;

            [SerializeField]
            public MissionPresenter presenter;
        }
    }
}