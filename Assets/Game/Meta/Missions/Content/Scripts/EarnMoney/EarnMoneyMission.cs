using System;
using Game.Gameplay.Player;
using GameSystem;
using Sirenix.OdinInspector;

namespace Game.Meta
{
    public class EarnMoneyMission : Mission
    {
        public override event Action<Mission> OnProgressChanged;

        [ReadOnly]
        [ShowInInspector]
        [PropertySpace(8)]
        public int EarnedMoney { get; private set; }

        [ReadOnly]
        [ShowInInspector]
        public int RequiredMoney
        {
            get { return config.RequiredMoney; }
        }

        public override float NormalizedProgress
        {
            get { return (float) EarnedMoney / RequiredMoney; }
        }

        public override string TextProgress
        {
            get { return $"{EarnedMoney}/{RequiredMoney}"; }
        }

        private readonly EarnMoneyMissionConfig config;

        [GameInject]
        private MoneyStorage moneyStorage;

        public EarnMoneyMission(EarnMoneyMissionConfig config) : base(config)
        {
            this.config = config;
            EarnedMoney = 0;
        }

        public void Setup(int currentResources)
        {
            EarnedMoney = Math.Min(currentResources, RequiredMoney);
        }

        protected override void OnStart()
        {
            moneyStorage.OnMoneyEarned += OnMoneyEarned;
        }

        protected override void OnStop()
        {
            moneyStorage.OnMoneyEarned -= OnMoneyEarned;
        }

        private void OnMoneyEarned(int income)
        {
            EarnedMoney = Math.Min(EarnedMoney + income, RequiredMoney);
            OnProgressChanged?.Invoke(this);
            TryComplete();
        }
    }
}