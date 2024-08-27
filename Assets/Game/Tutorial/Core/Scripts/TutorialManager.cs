using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Tutorial
{
    [AddComponentMenu("Tutorial/Tutorial Manager")]
    public sealed class TutorialManager : MonoBehaviour
    {
        public event Action<TutorialStep> OnStepFinished;

        public event Action<TutorialStep> OnNextStep;

        public event Action OnCompleted;

        public bool IsCompleted
        {
            get { return isCompleted; }
        }

        public TutorialStep CurrentStep
        {
            get { return stepList[currentIndex]; }
        }

        public int CurrentIndex
        {
            get { return currentIndex; }
        }

        internal static TutorialManager Instance { get; private set; }

        [SerializeField, FormerlySerializedAs("config")]
        private TutorialList stepList;

        private int currentIndex;

        private bool isCompleted;

        private void Awake()
        {
            if (Instance != null)
            {
                throw new Exception("TutorialManager is already created!");
            }

            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void Initialize(bool isCompleted = false, int stepIndex = 0)
        {
            this.isCompleted = isCompleted;
            currentIndex = Mathf.Clamp(stepIndex, 0, stepList.LastIndex);
        }

        public void FinishCurrentStep()
        {
            if (!isCompleted)
            {
                OnStepFinished?.Invoke(CurrentStep);
            }
        }

        public void MoveToNextStep()
        {
            if (isCompleted)
            {
                return;
            }

            if (stepList.IsLast(currentIndex))
            {
                isCompleted = true;
                OnCompleted?.Invoke();
                return;
            }

            currentIndex++;
            OnNextStep?.Invoke(CurrentStep);
        }

        public bool IsStepPassed(TutorialStep step)
        {
            if (isCompleted)
            {
                return true;
            }

            return stepList.IndexOf(step) < currentIndex;
        }

        public int IndexOfStep(TutorialStep step)
        {
            return stepList.IndexOf(step);
        }
    }
}