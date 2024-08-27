using System;
using Entities;
using Game.SceneAudio;
using Game.GameEngine.Mechanics;
using Game.Gameplay.Hero;
using GameSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Gameplay.Player
{
    [Serializable]
    public sealed class KillEnemyObserver :
        IGameInitElement,
        IGameReadyElement,
        IGameFinishElement
    {
        private HeroService heroService;

        private MoneyStorage moneyStorage;

        private MoneyPanelAnimator_AddJumpedMoney uiAnimator;

        private IComponent_MeleeCombat heroComponent;

        [SerializeField]
        private int minMoneyReward = 100;

        [SerializeField]
        private int maxMoneyReward = 300;

        [Space]
        [SerializeField]
        private AudioClip moneySFX;

        [GameInject]
        public void Construct(
            HeroService heroService,
            MoneyStorage moneyStorage,
            MoneyPanelAnimator_AddJumpedMoney uiAnimator
        )
        {
            this.heroService = heroService;
            this.moneyStorage = moneyStorage;
            this.uiAnimator = uiAnimator;
        }

        void IGameInitElement.InitGame()
        {
            heroComponent = heroService.GetHero().Get<IComponent_MeleeCombat>();
        }

        void IGameReadyElement.ReadyGame()
        {
            heroComponent.OnCombatStopped += OnCombatEnded;
        }

        void IGameFinishElement.FinishGame()
        {
            heroComponent.OnCombatStopped -= OnCombatEnded;
        }

        private void OnCombatEnded(CombatOperation operation)
        {
            if (operation.targetDestroyed)
            {
                AddMoneyReward(operation.targetEntity);
            }
        }

        private void AddMoneyReward(IEntity targetEnemy)
        {
            var reward = Random.Range(minMoneyReward, maxMoneyReward + 1);

            //Добавляем монеты в систему
            moneyStorage.EarnMoney(reward);

            //Добавляем монеты в UI через партиклы
            var particlePosiiton = targetEnemy.Get<IComponent_GetPosition>().Position;
            uiAnimator.PlayIncomeFromWorld(particlePosiiton, reward);

            //Звук
            SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, moneySFX);
        }
    }
}