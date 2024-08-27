using System;
using System.Collections;
using Elementary;
using GameSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Gameplay.Hero
{
    [Serializable]
    public abstract class TriggerVisitor<T> : TriggerObserver<T> where T : class
    {
        protected MonoBehaviour monoContext;
        
        [Space]
        [ReadOnly]
        [ShowInInspector]
        private T target;

        [ReadOnly]
        [ShowInInspector]
        protected bool IsVisiting { get; private set; }

        [SerializeField]
        private float checkConditionPeriod = 0.1f;

        private Coroutine updateRoutine;

        protected abstract bool CanEnter(T target);

        protected abstract ICondition ProvideConditions(T target);

        [GameInject]
        public void Construct(MonoBehaviour monoContext)
        {
            this.monoContext = monoContext;
        }

        protected sealed override void OnHeroEntered(T target)
        {
            if (this.target != null || !CanEnter(target))
            {
                return;
            }

            this.target = target;
            updateRoutine = monoContext.StartCoroutine(UpdateVisitState(target));
        }

        protected sealed override void OnHeroExited(T target)
        {
            if (!ReferenceEquals(this.target, target))
            {
                return;
            }

            if (updateRoutine != null)
            {
                monoContext.StopCoroutine(updateRoutine);
                updateRoutine = null;
            }

            if (IsVisiting)
            {
                IsVisiting = false;
                OnHeroQuit(this.target);
            }

            this.target = null;
        }

        protected abstract void OnHeroVisit(T target);

        protected abstract void OnHeroQuit(T target);

        private IEnumerator UpdateVisitState(T target)
        {
            WaitForSeconds period = null;
            
            if (checkConditionPeriod > 0.0f)
            {
                period = new WaitForSeconds(checkConditionPeriod);
            }

            var visitCondition = ProvideConditions(target);

            while (true)
            {
                var visitStarted = visitCondition.IsTrue();
                if (visitStarted && !IsVisiting)
                {
                    IsVisiting = true;
                    OnHeroVisit(this.target);
                }
                else if (!visitStarted && IsVisiting)
                {
                    IsVisiting = false;
                    OnHeroQuit(this.target);
                    visitCondition = ProvideConditions(target);
                }

                yield return period;
            }
        }
    }
}