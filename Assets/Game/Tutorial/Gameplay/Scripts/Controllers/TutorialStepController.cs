using GameSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Tutorial.Gameplay
{
    public abstract class TutorialStepController : MonoBehaviour,
        IGameConstructElement,
        IGameReadyElement,
        IGameStartElement,
        IGameFinishElement
    {
        [SerializeField]
        private TutorialStep step;

        private TutorialManager tutorialManager;

        public virtual void ConstructGame(GameContext context)
        {
            tutorialManager = TutorialManager.Instance;
        }

        public virtual void ReadyGame()
        {
            tutorialManager.OnStepFinished += CheckForFinish;
            tutorialManager.OnNextStep += CheckForStart;
        }

        public virtual void StartGame()
        {
            var stepFinished = tutorialManager.IsStepPassed(step);
            if (!stepFinished)
            {
                CheckForStart(tutorialManager.CurrentStep);
            }
        }

        public virtual void FinishGame()
        {
            tutorialManager.OnStepFinished -= CheckForFinish;
            tutorialManager.OnNextStep -= CheckForStart;
        }

        protected virtual void OnStart()
        {
        }

        protected virtual void OnStop()
        {
        }

        protected void NotifyAboutComplete()
        {
            if (tutorialManager.CurrentStep == step)
            {
                tutorialManager.FinishCurrentStep();
            }
        }

        protected void NotifyAboutMoveNext()
        {
            if (tutorialManager.CurrentStep == step)
            {
                tutorialManager.MoveToNextStep();
            }
        }

        protected void NotifyAboutCompleteAndMoveNext()
        {
            if (tutorialManager.CurrentStep == step)
            {
                tutorialManager.FinishCurrentStep();
                tutorialManager.MoveToNextStep();
            }
        }

        protected bool IsStepFinished()
        {
            return tutorialManager.IsStepPassed(step);
        }

        private void CheckForFinish(TutorialStep step)
        {
            if (this.step == step)
            {
                OnStop();
            }
        }

        private void CheckForStart(TutorialStep step)
        {
            if (this.step == step)
            {
                OnStart();
            }
        }
    }
}