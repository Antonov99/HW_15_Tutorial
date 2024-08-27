using System;
using Entities;
using Game.GameEngine.Mechanics;
using Game.Gameplay.Hero;
using GameSystem;
using Game.GameEngine;
using Sirenix.OdinInspector;

namespace Game.Meta
{
    public sealed class KillEnemyMission : Mission 
    {
        public override event Action<Mission> OnProgressChanged;

        [ReadOnly]
        [ShowInInspector]
        [PropertySpace(8)]
        public int CurrentKills { get; private set; }

        [ReadOnly]
        [ShowInInspector]
        public int RequiredKills
        {
            get { return config.RequiredKills; }
        }

        public override float NormalizedProgress
        {
            get { return (float) CurrentKills / RequiredKills; }
        }

        public override string TextProgress
        {
            get { return $"{CurrentKills}/{RequiredKills}"; }
        }

        private readonly KillEnemyMissionConfig config;

        [GameInject]
        private IHeroService heroService;

        public KillEnemyMission(KillEnemyMissionConfig config) : base(config)
        {
            this.config = config;
        }

        public void Setup(int currentKills)
        {
            CurrentKills = currentKills;
        }

        protected override void OnStart()
        {
            heroService.GetHero().Get<IComponent_MeleeCombat>().OnCombatStopped += OnCombatFinished;
        }

        protected override void OnStop()
        {
            heroService.GetHero().Get<IComponent_MeleeCombat>().OnCombatStopped -= OnCombatFinished;                        
        }

        private void OnCombatFinished(CombatOperation operation)
        {
            if (operation.targetDestroyed)
            {
                CurrentKills++;
                OnProgressChanged?.Invoke(this);
                TryComplete();
            }
        }
    }
}