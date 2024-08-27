using System;
using Game.GameEngine.Mechanics;
using Game.Gameplay.Hero;
using Game.SceneAudio;
using GameSystem;
using UnityEngine;

namespace Game.Gameplay.Player
{
    [Serializable]
    public sealed class HarvestResourceObserver :
        IGameInitElement,
        IGameReadyElement,
        IGameFinishElement
    {
        [SerializeField]
        private AudioClip collectSFX;

        private HeroService heroService;

        private ResourceStorage resourceStorage;

        private ResourcePanelAnimator_AddJumpedResources resourceAnimator;

        private IComponent_HarvestResource heroComponent;

        [GameInject]
        public void Construct(
            HeroService heroService,
            ResourceStorage resourceStorage,
            ResourcePanelAnimator_AddJumpedResources resourceAnimator
        )
        {
            this.heroService = heroService;
            this.resourceStorage = resourceStorage;
            this.resourceAnimator = resourceAnimator;
        }

        void IGameInitElement.InitGame()
        {
            heroComponent = heroService.GetHero().Get<IComponent_HarvestResource>();
        }

        void IGameReadyElement.ReadyGame()
        {
            heroComponent.OnHarvestStopped += OnHarvestStopped;
        }

        void IGameFinishElement.FinishGame()
        {
            heroComponent.OnHarvestStopped -= OnHarvestStopped;
        }

        private void OnHarvestStopped(HarvestResourceOperation operation)
        {
            if (operation.isCompleted)
            {
                AddResources(operation);
            }
        }

        private void AddResources(HarvestResourceOperation operation)
        {
            var resourceType = operation.resourceType;
            var resourceAmount = operation.resourceCount;
            resourceStorage.AddResource(resourceType, resourceAmount);

            var resourcePosition = operation.targetResource.Get<IComponent_GetPosition>().Position;
            resourceAnimator.PlayIncomeFromWorld(resourcePosition, resourceType, resourceAmount);

            SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, collectSFX);
            //
            // SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, this.collectSFX);
            // SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, this.collectSFX);
            // SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, this.collectSFX);
            // SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, this.collectSFX);
            // SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, this.collectSFX);
            // SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, this.collectSFX);
            // SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, this.collectSFX);
            // SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, this.collectSFX);
            // SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, this.collectSFX);
            // SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, this.collectSFX);
            // SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, this.collectSFX);
            // SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, this.collectSFX);
            // SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, this.collectSFX);
            // SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, this.collectSFX);
            // SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, this.collectSFX);
            // SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, this.collectSFX);
            // SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, this.collectSFX);
            // SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, this.collectSFX);
            // SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, this.collectSFX);
            // SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, this.collectSFX);
            // SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, this.collectSFX);
            // SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, this.collectSFX);
            // SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, this.collectSFX);
        }
    }
}