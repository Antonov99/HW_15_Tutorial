using System;
using System.Collections;
using UnityEngine;

namespace Game.Meta
{
    public sealed class BoosterViewAdapter
    {
        private readonly BoosterView view;

        private readonly Booster booster;

        private readonly MonoBehaviour coroutineDispatcher;

        private Coroutine timeCoroutine;

        public BoosterViewAdapter(BoosterView view, Booster booster, MonoBehaviour coroutineDispatcher)
        {
            this.view = view;
            this.booster = booster;
            this.coroutineDispatcher = coroutineDispatcher;
        }

        public void Show()
        {
            var metadata = booster.Metadata;
            view.SetIcon(metadata.icon);
            view.SetLabel(metadata.viewLabel);
            view.SetColor(metadata.viewColor);
            
            UpdateRemainingTime();

            timeCoroutine = coroutineDispatcher.StartCoroutine(UpdateTimeRoutine());
        }

        public void Hide()
        {
            if (timeCoroutine != null)
            {
                coroutineDispatcher.StopCoroutine(timeCoroutine);
                timeCoroutine = null;
            }
        }

        private IEnumerator UpdateTimeRoutine()
        {
            var period = new WaitForSeconds(1);
            while (true)
            {
                yield return period;
                UpdateRemainingTime();
            }
        }

        private void UpdateRemainingTime()
        {
            var remainingTime = booster.RemainingTime;
            var progress = remainingTime / booster.Duration;
            view.SetProgress(progress);

            var timeSpan = TimeSpan.FromSeconds(remainingTime);
            var remainingText = string.Format("{0:D1}h:{1:D2}m:{2:D2}s",
                timeSpan.Hours,
                timeSpan.Minutes,
                timeSpan.Seconds
            );
            view.SetRemainingText(remainingText);
        }
    }
}