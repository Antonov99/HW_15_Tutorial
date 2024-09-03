using System;
using System.Collections;
using Entities;
using GameSystem;
using UnityEngine;

namespace Game.Gameplay.Player
{
    public sealed class ConveyorVisitUnloadZoneObserver :
        IGameReadyElement,
        IGameFinishElement
    {
        public event Action OnResourcesUnloaded;
        
        private ConveyorVisitInteractor visitInteractor;

        private ResourceStorage resourceStorage;

        private ResourcePanelAnimator_AddResources uiAnimator;

        private MonoBehaviour monContext;

        private IComponent_UnloadZone targetUnloadZone;

        [GameInject]
        public void Construct(
            ConveyorVisitInteractor conveyorVisitInteractor,
            ResourceStorage resourceStorage,
            ResourcePanelAnimator_AddResources uiAnimator,
            MonoBehaviour monContext
        )
        {
            visitInteractor = conveyorVisitInteractor;
            this.resourceStorage = resourceStorage;
            this.uiAnimator = uiAnimator;
            this.monContext = monContext;
        }

        void IGameReadyElement.ReadyGame()
        {
            visitInteractor.OutputZone.OnEntered += OnConveyorEntered;
            visitInteractor.OutputZone.OnExited += OnConveyorExited;
        }

        void IGameFinishElement.FinishGame()
        {
            visitInteractor.OutputZone.OnEntered -= OnConveyorEntered;
            visitInteractor.OutputZone.OnExited -= OnConveyorExited;
        }

        private void OnConveyorEntered(IEntity entity)
        {
            targetUnloadZone = entity.Get<IComponent_UnloadZone>();
            targetUnloadZone.OnAmountChanged += OnAmountChanged;
            MoveResourcesFrom(targetUnloadZone);
        }

        private void OnConveyorExited(IEntity entity)
        {
            targetUnloadZone.OnAmountChanged -= OnAmountChanged;
            targetUnloadZone = null;
        }

        private void OnAmountChanged(int currentAmount)
        {
            monContext.StartCoroutine(MoveResourcesInNextFrame(targetUnloadZone));
        }

        private IEnumerator MoveResourcesInNextFrame(IComponent_UnloadZone unloadZone)
        {
            yield return new WaitForEndOfFrame();
            MoveResourcesFrom(unloadZone);
        }

        private void MoveResourcesFrom(IComponent_UnloadZone unloadZone)
        {
            if (unloadZone.IsEmpty)
            {
                return;
            }

            var resourceType = unloadZone.ResourceType;
            var income = unloadZone.ExtractAll();
            resourceStorage.AddResource(resourceType, income);
            uiAnimator.PlayIncomeFromWorld(unloadZone.ParticlePosition, resourceType, income);
            
            OnResourcesUnloaded?.Invoke();
        }
    }
}