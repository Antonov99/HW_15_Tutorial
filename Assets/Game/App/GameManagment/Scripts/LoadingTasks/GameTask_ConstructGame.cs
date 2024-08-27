using System;
using Services;

namespace Game.App
{
    public class GameTask_ConstructGame : ILoadingTask
    {
        private readonly GameFacade gameFacade;
        private readonly IGameLoadListener[] listeners;

        [ServiceInject]
        public GameTask_ConstructGame(GameFacade gameFacade, IGameLoadListener[] listeners)
        {
            this.gameFacade = gameFacade;
            this.listeners = listeners;
        }

        void ILoadingTask.Do(Action<LoadingResult> callback)
        {
            gameFacade.ConstructGame();

            foreach (var listener in listeners)
            {
                listener.OnLoadGame(gameFacade);
            }
            
            callback?.Invoke(LoadingResult.Success());
        }
    }
}