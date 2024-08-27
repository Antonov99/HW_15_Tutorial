using Game.App;
using Services;

namespace Game.Analytics
{
    public sealed class ApplicationAnalyticsTracker : 
        IAppStartListener,
        IAppQuitListener
    {
        [ServiceInject]
        private ApplicationManager applicationManager;

        void IAppStartListener.Start()
        {
            applicationManager.OnPaused += OnAppPaused;
            applicationManager.OnResumed += OnAppResumed;
            applicationManager.OnQuit += OnAppQuit;
            
            ApplicationAnalytics.LogApplicationStarted();
        }

        void IAppQuitListener.OnQuit()
        {
            applicationManager.OnPaused -= OnAppPaused;
            applicationManager.OnResumed -= OnAppResumed;
            applicationManager.OnQuit -= OnAppQuit;
        }

        private void OnAppPaused()
        {
            ApplicationAnalytics.LogApplicationPaused();
        }

        private void OnAppResumed()
        {
            ApplicationAnalytics.LogApplicationResumed();
        }

        private void OnAppQuit()
        {
            ApplicationAnalytics.LogApplicationExited();
        }
    }
}