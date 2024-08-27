using Game.GameEngine;
using Services;

namespace Game.App
{
    public sealed class RealtimeGameSynchronizer : 
        IAppStartListener,
        IAppQuitListener
    {
        [ServiceInject]
        private GameFacade gameFacade;
        
        [ServiceInject]
        private RealtimeClock realtimeClock;

        void IAppStartListener.Start()
        {
            realtimeClock.OnStarted += OnSessionStarted;
            realtimeClock.OnResumed += OnSessionResumed;
        }

        void IAppQuitListener.OnQuit()
        {
            realtimeClock.OnStarted -= OnSessionStarted;
            realtimeClock.OnResumed -= OnSessionResumed;
        }

        private void OnSessionStarted(long pauseSeconds)
        {
            if (pauseSeconds > 0)
            {
                gameFacade
                    .GetService<TimeShiftEmitter>()
                    .EmitEvent(TimeShiftReason.START_GAME, pauseSeconds);
            }
        }

        private void OnSessionResumed(long pauseSeconds)
        {
            if (pauseSeconds > 0)
            {
                gameFacade
                    .GetService<TimeShiftEmitter>()
                    .EmitEvent(TimeShiftReason.RESUME_GAME, pauseSeconds);
            }
        }
    }
}