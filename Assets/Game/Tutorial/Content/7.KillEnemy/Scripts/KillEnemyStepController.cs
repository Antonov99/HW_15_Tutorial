using Entities;
using Game.Tutorial.Gameplay;
using Game.Tutorial.UI;
using GameSystem;
using UnityEngine;

namespace Game.Tutorial
{
    [AddComponentMenu("Tutorial/Step «Kill Enemy»")]
    public sealed class KillEnemyStepController : TutorialStepController
    {
        private readonly KillEnemyInspector actionInspector = new();

        private ScreenTransform screenTransform;

        [SerializeField]
        private KillEnemyConfig config;

        [SerializeField]
        private KillEnemyManager enemyManager;

        [SerializeField]
        private KillEnemyPanelShower panelShower;

        public override void ConstructGame(GameContext context)
        {
            screenTransform = context.GetService<ScreenTransform>();

            enemyManager.Construct(context);
            panelShower.Construct(config);
            base.ConstructGame(context);
        }

        protected override async void OnStart()
        {
            var enemy = await enemyManager.SpawnEnemy();
            actionInspector.Inspect(enemy, OnEnemyDestroyed);
            panelShower.Show(screenTransform.Value);
        }

        private void OnEnemyDestroyed(IEntity enemy)
        {
            panelShower.Hide();
            StartCoroutine(enemyManager.DestroyEnemy(enemy as MonoEntity));
            NotifyAboutCompleteAndMoveNext();
        }
    }
}