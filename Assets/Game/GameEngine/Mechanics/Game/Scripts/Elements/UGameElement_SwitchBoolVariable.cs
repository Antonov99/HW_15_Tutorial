using Elementary;
using GameSystem;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Game/Game Element «Switch Bool Variable»")]
    public sealed class UGameElement_SwitchBoolVariable : MonoBehaviour,
        IGameStartElement,
        IGameFinishElement
    {
        [SerializeField]
        public MonoBoolVariable toggle;

        void IGameStartElement.StartGame()
        {
            toggle.SetValue(true);
        }

        void IGameFinishElement.FinishGame()
        {
            toggle.SetValue(false);
        }
    }
}