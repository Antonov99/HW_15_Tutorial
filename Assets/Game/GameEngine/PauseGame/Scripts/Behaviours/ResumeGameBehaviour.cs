using GameSystem;
using UnityEngine;

namespace Game.GameEngine
{
    public sealed class ResumeGameBehaviour : MonoBehaviour, IGameAttachElement
    {
        private GameContext gameContext;

        public void ResumeGame()
        {
            if (gameContext.CurrentState == GameContext.State.PAUSE)
            {
                gameContext.ResumeGame();
            }
        }

        void IGameAttachElement.AttachGame(GameContext context)
        {
            gameContext = context;
        }
    }
}