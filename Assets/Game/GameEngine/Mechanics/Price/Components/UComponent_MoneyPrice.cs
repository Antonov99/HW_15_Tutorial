using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Price/Component «Money Price»")]
    public sealed class UComponent_MoneyPrice : MonoBehaviour, IComponent_MoneyPrice
    {
        public int Price
        {
            get { return price; }
        }

        [SerializeField]
        private int price;
    }
}