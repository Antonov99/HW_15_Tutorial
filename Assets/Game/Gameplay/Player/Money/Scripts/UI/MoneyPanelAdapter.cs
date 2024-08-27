using GameSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Gameplay.Player
{
    public sealed class MoneyPanelAdapter : MonoBehaviour,
        IGameConstructElement,
        IGameInitElement,
        IGameReadyElement,
        IGameFinishElement
    {
        [SerializeField]
        private bool listenIncome = false;

        [SerializeField]
        private bool listenSpend = true;

        [SerializeField]
        private MoneyPanel panel;

        private MoneyStorage storage;

        void IGameConstructElement.ConstructGame(GameContext context)
        {
            storage = context.GetService<MoneyStorage>();
        }

        void IGameInitElement.InitGame()
        {
            panel.SetupMoney(storage.Money);
        }

        void IGameReadyElement.ReadyGame()
        {
            if (listenIncome)
            {
                storage.OnMoneyEarned += OnMoneyEarned;
            }

            if (listenSpend)
            {
                storage.OnMoneySpent += OnMoneySpent;
            }
        }

        void IGameFinishElement.FinishGame()
        {
            if (listenIncome)
            {
                storage.OnMoneyEarned -= OnMoneyEarned;
            }

            if (listenSpend)
            {
                storage.OnMoneySpent -= OnMoneySpent;
            }
        }

        private void OnMoneySpent(int range)
        {
            panel.DecrementMoney(range);
        }

        private void OnMoneyEarned(int range)
        {
            panel.IncrementMoney(range);
        }

#if UNITY_EDITOR

        [Button]
        private void Editor_UpdateMoney()
        {
            panel.SetupMoney(storage.Money);
        }
#endif
    }
}