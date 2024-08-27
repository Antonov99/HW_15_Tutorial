using Declarative;

namespace Game.GameEngine.Mechanics
{
    public sealed class HitPointsBarAdapterV1 :
        IAwakeListener,
        IEnableListener,
        IDisableListener
    {
        private IHitPoints hitPointsEngine;

        private HitPointsBar view;
        
        public void Construct(IHitPoints hitPointsEngine, HitPointsBar view)
        {
            this.hitPointsEngine = hitPointsEngine;
            this.view = view;
        }

        void IAwakeListener.Awake()
        {
            SetupBar();
        }

        void IEnableListener.OnEnable()
        {
            hitPointsEngine.OnCurrentPointsChanged += OnHitPointsChanged;
        }

        void IDisableListener.OnDisable()
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