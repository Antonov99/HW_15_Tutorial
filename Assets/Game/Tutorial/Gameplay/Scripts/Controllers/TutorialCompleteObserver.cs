using GameSystem;
using UnityEngine;

namespace Game.Tutorial.Gameplay
{
    public abstract class TutorialCompleteObserver : MonoBehaviour,
        IGameConstructElement,
        IGameReadyElement,
        IGameFinishElement
    {
        private TutorialManager tutorialManager;

        public virtual void ConstructGame(GameContext context)
        {
            tutorialManager = TutorialManager.Instance;
        }

        public virtual void ReadyGame()
        {
            tutorialManager.OnCompleted += OnTutorialComplete;
        }

        public virtual void FinishGame()
        {
            tutorialManager.OnCompleted -= OnTutorialComplete;
        }
        
        protected virtual void OnTutorialComplete()
        {
        }

        public bool IsCompleted()
        {
            return tutorialManager.IsCompleted;
        }
    }
}