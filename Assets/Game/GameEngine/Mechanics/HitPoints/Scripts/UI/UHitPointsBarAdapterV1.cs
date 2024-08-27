using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Hit Points/Hit Points Bar Adapter V1")]
    public sealed class UHitPointsBarAdapterV1 : MonoBehaviour
    {
        [SerializeField]
        private UHitPoints hitPointsEngine;

        [SerializeField]
        private HitPointsBar view;

        private void Awake()
        {
            SetupBar();
        }

        private void OnEnable()
        {
            hitPointsEngine.OnCurrentPointsChanged += OnHitPointsChanged;
        }

        private void OnDisable()
        {
            hitPointsEngine.OnCurrentPointsChanged -= OnHitPointsChanged;
        }

        private void OnHitPointsChanged(int hitPoints)
        {
            UpdateBar(hitPoints);
        }

        private void SetupBar()
        {
            var hitPoints = hitPointsEngine.Current;
            var maxHitPoints = hitPointsEngine.Max;
            
            var showBar = hitPoints > 0;
            view.SetVisible(showBar);
            view.SetHitPoints(hitPoints, maxHitPoints);
        }

        private void UpdateBar(int hitPoints)
        {
            var maxHitPoints = hitPointsEngine.Max;
            var showBar = hitPoints > 0;
            
            view.SetVisible(showBar);
            view.SetHitPoints(hitPoints, maxHitPoints);
        }
    }
}