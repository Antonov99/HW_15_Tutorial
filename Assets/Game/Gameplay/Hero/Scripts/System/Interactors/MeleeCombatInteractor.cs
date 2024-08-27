using System;
using System.Collections;
using Entities;
using Game.GameEngine.Mechanics;
using GameSystem;
using UnityEngine;

namespace Game.Gameplay.Hero
{
    [Serializable]
    public sealed class MeleeCombatInteractor : IGameInitElement
    {
        private HeroService heroService;

        private MonoBehaviour monoContext;

        [SerializeField]
        private float delay = 0.15f;

        private IComponent_MeleeCombat heroComponent;

        private Coroutine delayCoroutine;

        [GameInject]
        public void Construct(HeroService heroService, MonoBehaviour monoContext)
        {
            this.heroService = heroService;
            this.monoContext = monoContext;
        }

        public void TryStartCombat(IEntity target)
        {
            if (heroComponent.IsCombat)
            {
                return;
            }

            if (delayCoroutine == null)
            {
                delayCoroutine = monoContext.StartCoroutine(CombatRoutine(target));
            }
        }

        private IEnumerator CombatRoutine(IEntity target)
        {
            yield return new WaitForSeconds(delay);

            var operation = new CombatOperation(target);
            if (heroComponent.CanStartCombat(operation))
            {
                heroComponent.StartCombat(operation);
            }

            delayCoroutine = null;
        }

        void IGameInitElement.InitGame()
        {
            heroComponent = heroService.GetHero().Get<IComponent_MeleeCombat>();
        }
    }
}