using System;
using Elementary;
using Declarative;

namespace Game.Gameplay.Conveyors
{
    [Serializable]
    public sealed class InfoWidgetAdapter : 
        IAwakeListener,
        IEnableListener,
        IDisableListener
    {
        private ITimer workTimer;
        
        private InfoWidget view;

        public void Construct(ITimer workTimer, InfoWidget view)
        {
            this.workTimer = workTimer;
            this.view = view;
        }

        void IAwakeListener.Awake()
        {
            view.SetVisible(true);
            view.ProgressBar.SetVisible(workTimer.IsPlaying);
        }

        void IEnableListener.OnEnable()
        {
            workTimer.OnStarted += OnWorkStarted;
            workTimer.OnTimeChanged += OnWorkProgressChanged;
            workTimer.OnFinished += OnWorkFinished;
        }

        void IDisableListener.OnDisable()
        {
            workTimer.OnStarted -= OnWorkStarted;
            workTimer.OnTimeChanged -= OnWorkProgressChanged;
            workTimer.OnFinished -= OnWorkFinished;
        }

        private void OnWorkStarted()
        {
            view.ProgressBar.SetVisible(true);
        }

        private void OnWorkProgressChanged()
        {
            view.ProgressBar.SetProgress(workTimer.Progress);
        }

        private void OnWorkFinished()
        {
            view.ProgressBar.SetVisible(false);
        }
    }
}