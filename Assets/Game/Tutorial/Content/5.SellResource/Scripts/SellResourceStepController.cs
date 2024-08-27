using Game.Gameplay.Player;
using Game.Tutorial.App;
using Game.Tutorial.Gameplay;
using Game.Tutorial.UI;
using GameSystem;
using UnityEngine;

namespace Game.Tutorial
{
    [AddComponentMenu("Tutorial/Step «Sell Resource»")]
    public sealed class SellResourceStepController : TutorialStepController
    {
        private PointerManager pointerManager;

        private NavigationManager navigationManager;

        private ScreenTransform screenTransform;
    
        private readonly SellResourceInspector actionInspector = new();

        [SerializeField]
        private SellResourceConfig config;

        [SerializeField]
        private SellResourcePanelShower actionPanel = new();

        [SerializeField]
        private Transform pointerTransform;

        public override void ConstructGame(GameContext context)
        {
            pointerManager = context.GetService<PointerManager>();
            navigationManager = context.GetService<NavigationManager>();
            screenTransform = context.GetService<ScreenTransform>();
            
            var sellInteractor = context.GetService<VendorInteractor>();
            actionInspector.Construct(sellInteractor, config);
            actionPanel.Construct(config);
            
            base.ConstructGame(context);
        }

        protected override void OnStart()
        {
            TutorialAnalytics.LogEventAndCache("tutorial_step_3__cell_resource_started");
            actionInspector.Inspect(NotifyAboutCompleteAndMoveNext);

            var targetPosition = pointerTransform.position;
            pointerManager.ShowPointer(targetPosition, pointerTransform.rotation);
            navigationManager.StartLookAt(targetPosition);
            actionPanel.Show(screenTransform.Value);
        }

        protected override void OnStop()
        {
            TutorialAnalytics.LogEventAndCache("tutorial_step_3__cell_resource_completed");
            navigationManager.Stop();
            pointerManager.HidePointer();
            actionPanel.Hide();
        }
    }
}