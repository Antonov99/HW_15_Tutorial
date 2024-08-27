using System;
using System.Collections;
using Entities;
using Game.GameEngine.Mechanics;
using GameSystem;
using UnityEngine;

namespace Game.Gameplay.Hero
{
    [Serializable]
    public sealed class HarvestResourceInteractor : IGameInitElement
    {
        private HeroService heroService;

        private MonoBehaviour monoContext;

        [SerializeField]
        private float delay = 0.15f;

        private IComponent_HarvestResource heroComponent;

        private Coroutine delayCoroutine;

        [GameInject]
        public void Construct(HeroService heroService, MonoBehaviour monoContext)
        {
            this.heroService = heroService;
            this.monoContext = monoContext;
        }

        public void TryStartHarvest(IEntity resourceObject)
        {
            if (heroComponent.IsHarvesting)
            {
                return;
            }

            if (delayCoroutine == null)
            {
                delayCoroutine = monoContext.StartCoroutine(HarvestRoutine(resourceObject));
            }
        }

        private IEnumerator HarvestRoutine(IEntity resourceObject)
        {
            yield return new WaitForSeconds(delay);

            var operation = new HarvestResourceOperation(resourceObject);
            if (heroComponent.CanStartHarvest(operation))
            {
                heroComponent.StartHarvest(operation);
            }

            delayCoroutine = null;
        }

        void IGameInitElement.InitGame()
        {
            heroComponent = heroService.GetHero().Get<IComponent_HarvestResource>();
        }
    }
}