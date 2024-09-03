using Game.Gameplay.Hero;
using Game.Gameplay.Player;
using Game.Tutorial.Gameplay;
using Game.Tutorial.UI;
using GameSystem;
using UnityEngine;

namespace Game.Tutorial
{
    [AddComponentMenu("Tutorial/Step «Get Resource»")]
    public sealed class GetResourceStepController : TutorialStepController
    {
        private PointerManager pointerManager;
        
        private NavigationManager navigationManager;

        private ScreenTransform screenTransform;
        
        private readonly GetResourceInspector inspector = new();

        [SerializeField]
        private GetResourceConfig config;

        [SerializeField]
        private GetResourcePanelShower panelShower = new();

        [SerializeField]
        private Transform pointerTransform;

        public override void ConstructGame(GameContext context)
        {
            pointerManager = context.GetService<PointerManager>();
            navigationManager = context.GetService<NavigationManager>();
            screenTransform = context.GetService<ScreenTransform>();
            
            var conveyorVisitUnloadZoneObserver = context.GetService<ConveyorVisitUnloadZoneObserver>();
            inspector.Construct(conveyorVisitUnloadZoneObserver);
            
            panelShower.Construct(config);

            base.ConstructGame(context);
        }

        protected override void OnStart()
        {
            inspector.Inspect(callback: NotifyAboutCompleteAndMoveNext);
            
            var targetPosition = pointerTransform.position;
            pointerManager.ShowPointer(targetPosition, pointerTransform.rotation);
            navigationManager.StartLookAt(targetPosition);
            
            panelShower.Show(screenTransform.Value);
        }

        protected override void OnStop()
        {
            panelShower.Hide();
            pointerManager.HidePointer();
        }
    }
}