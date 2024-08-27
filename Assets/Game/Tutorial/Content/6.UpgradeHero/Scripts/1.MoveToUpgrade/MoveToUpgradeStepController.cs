using Game.Gameplay.Player;
using Game.Tutorial.Gameplay;
using Game.Tutorial.UI;
using GameSystem;
using UnityEngine;

namespace Game.Tutorial
{
    [AddComponentMenu("Tutorial/Step «Move To Upgrade»")]
    public sealed class MoveToUpgradeStepController : TutorialStepController, IGameInitElement
    {
        private PointerManager pointerManager;

        private NavigationManager navigationManager;

        private ScreenTransform screenTransform;
        
        private WorldPlacePopupShower worldPlacePopupShower;

        private readonly MoveToUpgradeInspector actionInspector = new();

        [SerializeField]
        private UpgradeHeroConfig config;

        [SerializeField]
        private MoveToUpgradePanelShower actionPanel;

        [SerializeField]
        private Transform pointerTransform;

        [SerializeField]
        private UpgradePopupShower popupShower;
        
        public override void ConstructGame(GameContext context)
        {
            pointerManager = context.GetService<PointerManager>();
            navigationManager = context.GetService<NavigationManager>();
            screenTransform = context.GetService<ScreenTransform>();
            worldPlacePopupShower = context.GetService<WorldPlacePopupShower>();

            var worldPlaceVisitor = context.GetService<WorldPlaceVisitInteractor>();
            actionInspector.Construct(worldPlaceVisitor, config);
            actionPanel.Construct(config);

            var popupManager = context.GetService<PopupManager>();
            popupShower.Construct(popupManager);

            base.ConstructGame(context);
        }

        void IGameInitElement.InitGame()
        {
            if (!IsStepFinished() && worldPlacePopupShower != null)
            {
                //Убираем базовый триггер
                worldPlacePopupShower.SetEnable(false);
            }
        }

        protected override void OnStart()
        {
            //Подписываемся на подход к месту:
            actionInspector.Inspect(OnPlaceVisited);

            //Показываем указатель:
            var targetPosition = pointerTransform.position;
            pointerManager.ShowPointer(targetPosition, pointerTransform.rotation);
            navigationManager.StartLookAt(targetPosition);

            //Показываем квест в UI:
            actionPanel.Show(screenTransform.Value);
        }

        private void OnPlaceVisited()
        {
            //Убираем указатель
            pointerManager.HidePointer();
            navigationManager.Stop();

            //Убираем квест из UI:
            actionPanel.Hide();

            //Показываем попап:
            popupShower.ShowPopup();
        }

        protected override void OnStop()
        {
            //Возвращаем базовый триггер:
            worldPlacePopupShower.SetEnable(true);
        }
    }
}