using System.Collections;
using Declarative;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    public sealed class HitPointsBarAdapterV2 :
        IAwakeListener,
        IEnableListener,
        IDisableListener
    {
        private IHitPoints hitPoints;

        private HitPointsBar view;

        private MonoBehaviour context;

        private Coroutine hideCoroutine;

        public void Construct(IHitPoints hitPoints, HitPointsBar view, MonoBehaviour context)
        {
            this.hitPoints = hitPoints;
            this.view = view;
            this.context = context;
        }

        void IAwakeListener.Awake()
        {
            SetupBar();
        }

        void IEnableListener.OnEnable()
        {
            hitPoints.OnSetuped += SetupBar;
            hitPoints.OnCurrentPointsChanged += UpdateBar;
        }

        void IDisableListener.OnDisable()
        {
            hitPoints.OnSetuped -= SetupBar;
            hitPoints.OnCurrentPointsChanged -= UpdateBar;
        }

        private void SetupBar()
        {
            ResetCoroutines();

            var hitPoints = this.hitPoints.Current;
            var maxHitPoints = this.hitPoints.Max;

            var showBar = hitPoints > 0 && hitPoints < maxHitPoints;
            view.SetVisible(showBar);
            view.SetHitPoints(hitPoints, maxHitPoints);
        }

        private void UpdateBar(int hitPoints)
        {
            ResetCoroutines();

            var maxHitPoints = this.hitPoints.Max;

            view.SetVisible(true);
            view.SetHitPoints(hitPoints, maxHitPoints);

            if (hitPoints <= 0 || hitPoints == maxHitPoints)
            {
                hideCoroutine = context.StartCoroutine(HideWithDelay());
            }
        }

        private void ResetCoroutines()
        {
            if (hideCoroutine != null)
            {
                context.StopCoroutine(hideCoroutine);
                hideCoroutine = null;
            }
        }

        private IEnumerator HideWithDelay()
        {
            yield return new WaitForSeconds(1.0f);
            view.SetVisible(false);
            hideCoroutine = null;
        }
    }
}