using System;
using System.Collections;
using Entities;
using GameSystem;
using UnityEngine;

namespace Game.Gameplay.Player
{
    public sealed class ConveyorVisitInputZoneObserver :
        IGameReadyElement,
        IGameFinishElement
    {
        private ConveyorVisitInteractor visitInteractor;

        private ResourceStorage resourceStorage;

        private MonoBehaviour monoContext;

        private IComponent_LoadZone targetLoadZone;
        
        [GameInject]
        public void Construct(
            ConveyorVisitInteractor conveyorVisitInteractor,
            ResourceStorage resourceStorage,
            MonoBehaviour monoContext
        )
        {
            visitInteractor = conveyorVisitInteractor;
            this.resourceStorage = resourceStorage;
            this.monoContext = monoContext;
        }

        void IGameReadyElement.ReadyGame()
        {
            visitInteractor.InputZone.OnEntered += OnConveyorEntered;
            visitInteractor.InputZone.OnExited += OnConveyorExited;
        }

        void IGameFinishElement.FinishGame()
        {
            visitInteractor.InputZone.OnEntered -= OnConveyorEntered;
            visitInteractor.InputZone.OnExited -= OnConveyorExited;
        }

        private void OnConveyorEntered(IEntity entity)
        {
            targetLoadZone = entity.Get<IComponent_LoadZone>();
            targetLoadZone.OnAmountChanged += OnAmountChanged;
            MoveResourcesTo(targetLoadZone);
        }

        private void OnConveyorExited(IEntity entity)
        {
            targetLoadZone.OnAmountChanged -= OnAmountChanged;
            targetLoadZone = null;
        }

        private void OnAmountChanged(int currentAmount)
        {
            monoContext.StartCoroutine(MoveResourcesInNextFrame(targetLoadZone));
        }

        private IEnumerator MoveResourcesInNextFrame(IComponent_LoadZone loadZone)
        {
            yield return new WaitForEndOfFrame();
            MoveResourcesTo(loadZone);
        }

        private void MoveResourcesTo(IComponent_LoadZone loadZone)
        {
            if (loadZone.IsFull)
            {
                return;
            }

            var resourceType = loadZone.ResourceType;
            var resourcesInStorage = resourceStorage.GetResource(resourceType);
            if (resourcesInStorage <= 0)
            {
                return;
            }

            var resultAmount = Math.Min(resourcesInStorage, loadZone.AvailableAmount);
            resourceStorage.ExtractResource(resourceType, resultAmount);
            loadZone.PutAmount(resultAmount);
        }
    }
}