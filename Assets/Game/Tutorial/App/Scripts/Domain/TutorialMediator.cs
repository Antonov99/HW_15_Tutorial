using Game.App;
using Newtonsoft.Json;
using Services;
using UnityEngine;

namespace Game.Tutorial.App
{
    public class TutorialMediator :
        IAppInitListener,
        IAppStartListener,
        IAppQuitListener
    {
        private const string TUTORIAL_PREFS_KEY = "TutorialData";

        [ServiceInject]
        protected TutorialManager tutorialManager;

        void IAppInitListener.Init()
        {
            SetupData();
        }

        protected virtual void SetupData()
        {
            if (PlayerPrefs.HasKey(TUTORIAL_PREFS_KEY))
            {
                var json = PlayerPrefs.GetString(TUTORIAL_PREFS_KEY);
                var data = JsonUtility.FromJson<TutorialData>(json);
                tutorialManager.Initialize(data.isCompleted, data.stepIndex);
            }
            else
            {
                tutorialManager.Initialize();
            }
        }

        void IAppStartListener.Start()
        {
            tutorialManager.OnStepFinished += OnTutorialStepFinished;
            tutorialManager.OnCompleted += OnTutorialCompleted;
        }

        void IAppQuitListener.OnQuit()
        {
            tutorialManager.OnStepFinished -= OnTutorialStepFinished;
            tutorialManager.OnCompleted -= OnTutorialCompleted;
        }

        private void OnTutorialStepFinished(TutorialStep step)
        {
            var nextStepIndex = tutorialManager.IndexOfStep(step) + 1;

            var data = new TutorialData
            {
                isCompleted = false,
                stepIndex = nextStepIndex
            };

            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(TUTORIAL_PREFS_KEY, json);
        }

        private void OnTutorialCompleted()
        {
            var data = new TutorialData
            {
                isCompleted = true,
                stepIndex = tutorialManager.IndexOfStep(tutorialManager.CurrentStep)
            };

            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(TUTORIAL_PREFS_KEY, json);
        }
    }
}

