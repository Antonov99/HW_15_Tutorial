using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable EventNeverSubscribedTo.Global

namespace AI.GOAP
{
    [AddComponentMenu("AI/GOAP/Goal Oriented Agent")]
    [DisallowMultipleComponent]
    public class GoalOrientedAgent : MonoBehaviour, IActor.Callback
    {
        public event Action OnStarted;
        public event Action OnFailed;
        public event Action OnCanceled;
        public event Action OnCompleted;

        [SerializeField, PropertyOrder(-11), HideInPlayMode]
        private PlannerMode plannerMode;

        private IPlanner planner;

        [SerializeField, Space, PropertyOrder(-9), HideInPlayMode]
        private Goal[] goals;

        [SerializeField, Space, PropertyOrder(-8), HideInPlayMode]
        private Actor[] actions;

        [SerializeField, Space, PropertyOrder(-7), HideInPlayMode]
        private FactInspector[] factInspectors;

        [Title("Debug")]
        [ShowInInspector, ReadOnly, PropertySpace, PropertyOrder(-6)]
        public bool IsPlaying
        {
            get { return currentPlan != null; }
        }

        [ShowInInspector, ReadOnly, PropertyOrder(-5)]
        public IGoal CurrentGoal
        {
            get { return currentGoal; }
        }

        public FactState WorldState
        {
            get { return GenerateWorldState(); }
        }

        public IEnumerable<IGoal> Goals
        {
            get { return goals; }
        }

        public IEnumerable<IActor> Actions
        {
            get { return actions; }
        }
        
        private IGoal currentGoal;

        [ShowInInspector, ReadOnly, PropertyOrder(-5)]
        private List<IActor> currentPlan;

        [ShowInInspector, ReadOnly, PropertyOrder(-4)]
        private int actionIndex;

        private void Awake()
        {
            planner = PlannerFactory.CreatePlanner(plannerMode);
        }

        public void Play()
        {
            var goal = goals
                .Where(it => it.IsValid())
                .OrderByDescending(it => it.EvaluatePriority())
                .FirstOrDefault();

            if (goal == null)
            {
                Debug.LogWarning("Can't play: no valid goals!");
                return;
            }

            var actions = this.actions
                .Where(it => it.IsValid())
                .ToArray<IActor>();

            if (actions.Length <= 0)
            {
                Debug.LogWarning("Can't play: no valid actions!");
                return;
            }

            if (!planner.MakePlan(WorldState, goal.ResultState, actions, out var plan))
            {
                Debug.LogWarning($"Can't make a plan for goal {goal.name}!");
                return;
            }

            if (plan.Count <= 0)
            {
                Debug.LogWarning($"Plan for goal {goal.name} is empty!");
                return;
            }

            currentGoal = goal;
            currentPlan = plan;

            actionIndex = 0;
            OnStarted?.Invoke();

            PlayAction();
        }

        public void Cancel()
        {
            StopAllCoroutines();
            
            if (currentPlan != null && 
                actionIndex < currentPlan.Count)
            {
                currentPlan[actionIndex].Cancel();
            }

            currentPlan = null;
            actionIndex = 0;
            OnCancel();
        }

        public void Replay()
        {
            Cancel();
            Play();
        }
        
        private void PlayAction()
        {
            var action = currentPlan[actionIndex];
            if (!action.IsValid())
            {
                Fail();
                return;
            }

            if (!action.RequiredState.EqualsTo(WorldState))
            {
                Fail();
                return;
            }

            action.Play(callback: this);
        }

        void IActor.Callback.Invoke(IActor action, bool success)
        {
            if (!success)
            {
                Fail();
                return;
            }

            if (!action.ResultState.EqualsTo(WorldState))
            {
                Fail();
                return;
            }

            var planCompleted = actionIndex + 1 >= currentPlan.Count;
            if (planCompleted)
            {
                Complete();
                return;
            }

            actionIndex++;
            StartCoroutine(PlayNextAction());
        }

        private IEnumerator PlayNextAction()
        {
            yield return new WaitForFixedUpdate();
            PlayAction();
        }

        private void Fail()
        {
            currentPlan = null;
            actionIndex = 0;
            OnFail();
        }

        private void Complete()
        {
            currentPlan = null;
            actionIndex = 0;
            OnComplete();
        }
        
        private FactState GenerateWorldState()
        {
            var worldState = new FactState();
            
            for (int i = 0, count = factInspectors.Length; i < count; i++)
            {
                var inspector = factInspectors[i];
                inspector.PopulateFacts(worldState);
            }

            return worldState;
        }

        protected virtual void OnFail()
        {
            OnFailed?.Invoke();
        }

        protected virtual void OnComplete()
        {
            OnCompleted?.Invoke();
        }

        protected virtual void OnCancel()
        {
            OnCanceled?.Invoke();
        }
    }
}