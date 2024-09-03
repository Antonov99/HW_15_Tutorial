using Game.Gameplay.Player;
using Game.Meta;
using Game.Tutorial.Gameplay;
using Game.Tutorial.UI;
using GameSystem;
using UnityEngine;

namespace Game.Tutorial
{
    [AddComponentMenu("Tutorial/Step «Get Reward»")]
    public sealed class RewardStepController : TutorialStepController
    {
        private PointerManager pointerManager;

        private NavigationManager navigationManager;

        private ScreenTransform screenTransform;

        private readonly MoveToRewardInspector actionInspector = new();

        private readonly GetRewardInspector getRewardInspector = new();

        [SerializeField]
        private RewardConfig config;

        [SerializeField]
        private MoveToRewardPanelShower actionPanel;

        [SerializeField]
        private Transform pointerTransform;

        public override void ConstructGame(GameContext context)
        {
            pointerManager = context.GetService<PointerManager>();
            navigationManager = context.GetService<NavigationManager>();
            screenTransform = context.GetService<ScreenTransform>();

            var worldPlaceVisitor = context.GetService<WorldPlaceVisitInteractor>();
            actionInspector.Construct(worldPlaceVisitor, config);

            var missionsManager = context.GetService<MissionsManager>();
            getRewardInspector.Construct(missionsManager, config);

            actionPanel.Construct(config);

            base.ConstructGame(context);
        }

        protected override void OnStart()
        {
            actionInspector.Inspect(OnPlaceVisited);

            var targetPosition = pointerTransform.position;
            pointerManager.ShowPointer(targetPosition, pointerTransform.rotation);
            navigationManager.StartLookAt(targetPosition);

            actionPanel.Show(screenTransform.Value);
        }

        private void OnPlaceVisited()
        {
            pointerManager.HidePointer();
            navigationManager.Stop();

            actionPanel.Hide();
            getRewardInspector.Inspect(OnRewardReceived);
            NotifyAboutComplete();
        }

        private void OnRewardReceived()
        {
            NotifyAboutMoveNext();
        }
    }
}