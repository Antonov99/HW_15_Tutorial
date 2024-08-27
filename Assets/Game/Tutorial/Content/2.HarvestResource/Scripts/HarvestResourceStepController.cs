using Game.Gameplay.Hero;
using Game.Tutorial.App;
using Game.Tutorial.Gameplay;
using Game.Tutorial.UI;
using GameSystem;
using Services;
using UnityEngine;

namespace Game.Tutorial
{
    [AddComponentMenu("Tutorial/Step «Harvest Resource»")]
    public sealed class HarvestResourceStepController : TutorialStepController
    {
        private PointerManager pointerManager;

        private ScreenTransform screenTransform;
        
        private readonly HarvestResourceInspector inspector = new();

        [SerializeField]
        private HarvestResourceConfig config;

        [SerializeField]
        private HarvestResourcePanelShower panelShower = new();

        [SerializeField]
        private Transform pointerTransform;

        public override void ConstructGame(GameContext context)
        {
            pointerManager = context.GetService<PointerManager>();
            screenTransform = context.GetService<ScreenTransform>();

            var heroService = context.GetService<IHeroService>();
            inspector.Construct(heroService, config);
            panelShower.Construct(config);

            base.ConstructGame(context);
        }

        protected override void OnStart()
        {
            TutorialAnalytics.LogEventAndCache("tutorial_step_2__harvest_resource_started");
            inspector.Inspect(callback: NotifyAboutCompleteAndMoveNext);
            pointerManager.ShowPointer(pointerTransform.position, pointerTransform.rotation);
            panelShower.Show(screenTransform.Value);
        }

        protected override void OnStop()
        {
            TutorialAnalytics.LogEventAndCache("tutorial_step_2__harvest_resource_completed");
            panelShower.Hide();
            pointerManager.HidePointer();
        }
    }
}