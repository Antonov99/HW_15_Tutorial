using Services;

namespace Game.App
{
    public sealed class GameSaver :
        IGameStartListener,
        IGameStopListener
    {
        private const float SAVE_PERIOD_IN_SECONDS = 30;

        private ApplicationManager appManager;

        private GameRepository gameRepository;

        private IGameMediator[] mediators;
        
        private float remainingSeconds;
        
        public bool IsPaused { get; set; }

        [ServiceInject]
        public void Construct(ApplicationManager appManager, GameRepository gameRepository, IGameMediator[] mediators)
        {
            this.appManager = appManager;
            this.gameRepository = gameRepository;
            this.mediators = mediators;
        }

        void IGameStartListener.OnStartGame(GameFacade gameFacade)
        {
            remainingSeconds = SAVE_PERIOD_IN_SECONDS;

            appManager.OnUpdate += OnApplicationUpdate;
            appManager.OnPaused += OnApplicationPaused;
            appManager.OnQuit += OnQuitApplication;
        }

        void IGameStopListener.OnStopGame(GameFacade gameFacade)
        {
            appManager.OnUpdate -= OnApplicationUpdate;
            appManager.OnPaused -= OnApplicationPaused;
            appManager.OnQuit -= OnQuitApplication;
        }

        private void OnApplicationUpdate(float deltaTime)
        {
            remainingSeconds -= deltaTime;
            if (remainingSeconds <= 0.0f)
            {
                Save();
            }
        }

        private void OnApplicationPaused()
        {
            Save();
        }

        private void OnQuitApplication()
        {
            Save();
        }

        public void Save()
        {
            if (IsPaused)
            {
                return;
            }
            
            for (int i = 0, count = mediators.Length; i < count; i++)
            {
                var mediator = mediators[i];
                mediator.SaveData(gameRepository);
            }

            gameRepository.SaveAllStates();
            remainingSeconds = SAVE_PERIOD_IN_SECONDS;
        }
    }
}