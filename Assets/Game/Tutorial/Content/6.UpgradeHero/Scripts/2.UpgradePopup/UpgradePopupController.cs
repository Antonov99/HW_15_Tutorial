using Game.Meta;
using Game.Tutorial.Gameplay;
using GameSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Tutorial
{
    [AddComponentMenu("Tutorial/Step «Upgrade Popup»")]
    public sealed class UpgradePopupController : TutorialStepController
    {
        private readonly UpgradeInspector questInspector = new();

        [SerializeField]
        private UpgradeHeroConfig config;

        [SerializeField]
        private GameObject questCursor;

        [SerializeField]
        private Transform fading;

        [Header("Close")]
        [SerializeField]
        private Button closeButton;

        [SerializeField]
        private GameObject closeCursor;

        private void Awake()
        {
            questCursor.SetActive(false);
            closeCursor.SetActive(false);
            closeButton.interactable = false;
        }

        public override void ConstructGame(GameContext context)
        {
            var upgradesManager = context.GetService<UpgradesManager>();
            questInspector.Construct(upgradesManager, config);

            base.ConstructGame(context);
        }

        public void Show()
        {
            //Ждем выполнение квеста прокачки:
            questInspector.Inspect(OnQuestFinished);
            
            //Включаем курсор на прокачке:
            questCursor.SetActive(true);
        }

        private void OnQuestFinished()
        {
            //Выключаем курсор на прокачке:
            questCursor.SetActive(false);

            //Включаем курсор на кнопке закрыть:
            closeCursor.SetActive(true);

            //Делаем затемнение на прокачке:
            fading.SetAsLastSibling();

            //Активируем кнопку закрыть:
            closeButton.interactable = true;
            closeButton.onClick.AddListener(OnCloseClicked);
            
            //Завершаем шаг туториала:
            NotifyAboutComplete();
        }
        
        private void OnCloseClicked()
        {
            closeButton.onClick.RemoveListener(OnCloseClicked);
            
            //Выключаем курсор на кнопке закрыть:
            closeCursor.SetActive(false);

            //Переходим к следующему шагу туториала:
            NotifyAboutMoveNext();
        }
    }
}