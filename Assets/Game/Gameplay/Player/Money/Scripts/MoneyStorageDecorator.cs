using GameSystem;
using UnityEngine;

namespace Game.Gameplay.Player
{
    [AddComponentMenu("Gameplay/Player Money Storage Decorator")]
    public sealed class MoneyStorageDecorator : MonoBehaviour, IGameConstructElement
    {
        private MoneyStorage storage;
        private MoneyPanel view;
        
        //TODO ANIMATORS...

        public void EarnMoneySimple(int money)
        {
            storage.EarnMoney(money);
            view.IncrementMoney(money);
        }

        void IGameConstructElement.ConstructGame(GameContext context)
        {
            storage = context.GetService<MoneyStorage>();
            view = context.GetService<MoneyPanel>();
        }
    }
}