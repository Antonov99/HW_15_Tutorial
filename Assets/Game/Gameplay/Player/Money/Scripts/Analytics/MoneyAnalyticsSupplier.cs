using GameSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Gameplay.Player
{
    public sealed class MoneyAnalyticsSupplier : MonoBehaviour,
        IGameConstructElement,
        IGameReadyElement,
        IGameFinishElement
    {
        [PropertyOrder(-10)]
        [ReadOnly]
        [ShowInInspector]
        public int PreviousMoney { get; private set; }

        [PropertyOrder(-9)]
        [ReadOnly]
        [ShowInInspector]
        public int CurrentMoney { get; private set; }

        [Space]
        [SerializeField]
        private MoneyStorage storage;

        void IGameConstructElement.ConstructGame(GameContext context)
        {
            PreviousMoney = storage.Money;
            CurrentMoney = storage.Money;
        }

        void IGameReadyElement.ReadyGame()
        {
            storage.OnMoneyChanged += OnMoneyChanged;
        }

        void IGameFinishElement.FinishGame()
        {
            storage.OnMoneyChanged -= OnMoneyChanged;
        }

        private void OnMoneyChanged(int newValue)
        {
            PreviousMoney = CurrentMoney;
            CurrentMoney = newValue;
        }
    }
}