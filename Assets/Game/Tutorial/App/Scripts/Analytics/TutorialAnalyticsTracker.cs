using Game.App;

namespace Game.Tutorial.App
{
    public sealed class TutorialAnalyticsTracker : IGameStartListener
    {
        void IGameStartListener.OnStartGame(GameFacade gameFacade)
        {
            var tutorialManager = TutorialManager.Instance;
            if (tutorialManager.IsCompleted)
            {
                return;
            }

            tutorialManager.OnCompleted += OnCompleteTutorial;
            TutorialAnalytics.LogTutorialStarted(); //1 раз
        }

        private void OnCompleteTutorial()
        {
            TutorialManager.Instance.OnCompleted -= OnCompleteTutorial;
            TutorialAnalytics.LogTutorialCompleted(); //1 раз
        }
    }
}