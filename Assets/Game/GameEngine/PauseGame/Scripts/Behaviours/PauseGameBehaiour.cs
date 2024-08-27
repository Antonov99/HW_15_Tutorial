using GameSystem;
using UnityEngine;

namespace Game.GameEngine
{
    public sealed class PauseGameBehaiour : MonoBehaviour, IGameAttachElement
    {
        private GameContext gameContext;

        public void PauseGame()
        {
            if (gameContext.CurrentState == GameContext.State.PLAY)
            {
                gameContext.PauseGame();
            }
        }

        void IGameAttachElement.AttachGame(GameContext context)
        {
            gameContext = context;
        }
    }
}