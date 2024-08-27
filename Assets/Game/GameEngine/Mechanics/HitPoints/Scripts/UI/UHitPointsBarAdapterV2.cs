using System.Collections;
using Game.GameEngine.Mechanics;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Hit Points/Hit Points Bar Adapter V2")]
    public sealed class UHitPointsBarAdapterV2 : MonoBehaviour
    {
        [SerializeField]
        private UHitPoints hitPointsEngine;

        [SerializeField]
        private HitPointsBar view;

        private Coroutine hideCoroutine;

        private void Awake()
        {
            SetupBar();
        }

        private void OnEnable()
        {
            hitPointsEngine.OnSetuped += SetupBar;
            hitPointsEngine.OnCurrentPointsChanged += UpdateBar;
        }

        private void OnDisable()
        {
            hitPointsEngine.OnSetuped -= SetupBar;
            hitPointsEngine.OnCurrentPointsChanged -= UpdateBar;
        }

        private void SetupBar()
        {
            ResetCoroutines();

            var hitPoints = hitPointsEngine.Current;
            var maxHitPoints = hitPointsEngine.Max;
            
            var showBar = hitPoints > 0 && hitPoints < maxHitPoints;
            view.SetVisible(showBar);
            view.SetHitPoints(hitPoints, maxHitPoints);
        }

        private void UpdateBar(int hitPoints)
        {
            ResetCoroutines();

            var maxHitPoints = hitPointsEngine.Max;
            
            view.SetVisible(true);
            view.SetHitPoints(hitPoints, maxHitPoints);

            if (hitPoints <= 0 || hitPoints == maxHitPoints)
            {
                hideCoroutine = StartCoroutine(HideWithDelay());
            }
        }
        
        private void ResetCoroutines()
        {
            if (hideCoroutine != null)
            {
                StopCoroutine(hideCoroutine);
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