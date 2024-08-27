using Entities;
using GameSystem;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Game/Game Element «Switch Enable Components»")]
    public sealed class UGameElement_SwitchEnableComponents : MonoBehaviour,
        IGameStartElement,
        IGameFinishElement
    {
        [SerializeField]
        public MonoEntity[] units;

        void IGameStartElement.StartGame()
        {
            EnableUnits(true);
        }

        void IGameFinishElement.FinishGame()
        {
            EnableUnits(false);
        }

        private void EnableUnits(bool isEnable)
        {
            for (int i = 0, count = units.Length; i < count; i++)
            {
                var unit = units[i];
                unit.Get<IComponent_Enable>().SetEnable(isEnable);
            }
        }
    }
}