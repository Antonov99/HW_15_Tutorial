using Game.UI;
using UnityEngine;

namespace Game.GameEngine
{
    [AddComponentMenu("GameEngine/Mechanics/Time/Timer Mechanics «Update Progress Bar»")]
    public sealed class UTimerMechanics_UpdateProgressBar : UTimerMechanics
    {
        [SerializeField]
        public ProgressBar progressBar;

        #region Lifecycle

        private void Awake()
        {
            progressBar.SetVisible(false);
        }

        #endregion

        #region Callbacks

        protected override void OnTimerStarted()
        {
            progressBar.SetProgress(timer.Progress);
        }

        protected override void OnTimeChanged()
        {
            var progress = timer.Progress;
            progressBar.SetProgress(progress);
        }

        protected override void OnTimerFinished()
        {
            progressBar.SetProgress(1.0f);
        }

        protected override void OnTimerCanceled()
        {
            progressBar.SetVisible(false);
        }

        #endregion
    }
}