using Game.Gameplay.Hero;
using Game.Tutorial.Gameplay;
using Game.Tutorial.UI;
using GameSystem;
using UnityEngine;

namespace Game.Tutorial
{
    [AddComponentMenu("Tutorial/Step «Convert Resource»")]
    public sealed class ConvertResourceStepController : TutorialStepController
    {
        private PointerManager pointerManager;

        private ScreenTransform screenTransform;
        
        private readonly ConvertResourceInspector inspector = new();

        [SerializeField]
        private ConvertResourceConfig config;

        [SerializeField]
        private ConvertResourcePanelShower panelShower = new();

        [SerializeField]
        private Transform pointerTransform;

        public override void ConstructGame(GameContext context)
        {
            pointerManager = context.GetService<PointerManager>();
            screenTransform = context.GetService<ScreenTransform>();

            var heroService = context.GetService<IHeroService>();
            inspector.Construct(heroService);
            panelShower.Construct(config);

            base.ConstructGame(context);
        }

        protected override void OnStart()
        {
            inspector.Inspect(callback: NotifyAboutCompleteAndMoveNext);
            pointerManager.ShowPointer(pointerTransform.position, pointerTransform.rotation);
            panelShower.Show(screenTransform.Value);
        }

        protected override void OnStop()
        {
            panelShower.Hide();
            pointerManager.HidePointer();
        }
    }
}