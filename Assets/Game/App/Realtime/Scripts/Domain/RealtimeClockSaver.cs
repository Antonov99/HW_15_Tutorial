using Services;

namespace Game.App
{
    public sealed class RealtimeClockSaver : 
        IAppStartListener,
        IAppQuitListener
    {
        [ServiceInject]
        private RealtimePreferences preferences;

        [ServiceInject]
        private RealtimeClock realtimeClock;
        
        void IAppStartListener.Start()
        {
            realtimeClock.OnPaused += SaveSession;
            realtimeClock.OnEnded += SaveSession;
        }

        void IAppQuitListener.OnQuit()
        {
            realtimeClock.OnPaused -= SaveSession;
            realtimeClock.OnEnded -= SaveSession;
        }

        private void SaveSession()
        {
            var data = new RealtimeData
            {
                nowSeconds = realtimeClock.RealtimeSeconds
            };
            preferences.SaveData(data);
        }
    }
}