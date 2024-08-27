using System.Collections;
using CustomParticles;
using Game.GameEngine;
using Game.GameEngine.GameResources;
using Game.GameEngine.GUI;
using Game.UI;
using GameSystem;
using UnityEngine;

namespace Game.Gameplay.Player
{
    public sealed class ResourcePanelAnimator_AddResources : MonoBehaviour, IGameConstructElement
    {
        [Space]
        [SerializeField]
        private ResourcePanel panel;

        [SerializeField]
        private ResourceInfoCatalog resourceCatalog;

        [Space]
        [SerializeField]
        private int maxParticleCount;

        [SerializeField]
        private float emissonPeriod;

        [SerializeField]
        private UIAnimations.FlySettings settings;

        private ParticlePool<ImageParticle> particlePool;

        private Transform worldViewport;

        private Camera worldCamera;

        private Camera uiCamera;

        public void PlayIncomeFromWorld(Vector3 startWorldPosition, ResourceType resourceType, int income)
        {
            StartCoroutine(PlayFromWorldRoutine(startWorldPosition, resourceType, income));
        }

        private IEnumerator PlayFromWorldRoutine(Vector3 startWorldPosition, ResourceType resourceType, int income)
        {
            if (!panel.IsItemShown(resourceType))
            {
                panel.ShowItem(resourceType);
                yield return new WaitForEndOfFrame(); //Wait for calc UI position    
            }

            var info = resourceCatalog.FindResource(resourceType);
            var icon = info.icon;

            var prevValue = panel.GetCurrentValue(resourceType);
            var newValue = prevValue + income;
            var particleIterator = new IntValueIterator(prevValue, newValue, maxParticleCount);

            var emissionPeriod = new WaitForSeconds(emissonPeriod);
            var endUIPosition = panel.GetIconCenter(resourceType);

            for (int i = 0, count = particleIterator.ParticleCount; i < count; i++)
            {
                var routine = PlayParticle(
                    startWorldPosition,
                    endUIPosition,
                    icon,
                    resourceType,
                    particleIterator
                );
                StartCoroutine(routine);
                yield return emissionPeriod;
            }
        }

        private IEnumerator PlayParticle(
            Vector3 startWorldPosition,
            Vector3 endUIPosiiton,
            Sprite icon,
            ResourceType resourceType,
            IntValueIterator particleIterator
        )
        {
            var particleObject = particlePool.Get(worldViewport);
            particleObject.SetIcon(icon);

            var particleTransform = particleObject.transform;
            particleTransform.position = CameraUtils.FromWorldToUIPosition(
                worldCamera,
                uiCamera,
                startWorldPosition
            );

            yield return UIAnimations.AnimateFlyRoutine(particleTransform, settings, endUIPosiiton);
            particlePool.Release(particleObject);

            if (particleIterator.NextValue(out var resourceCount))
            {
                panel.IncrementItem(resourceType, resourceCount);
            }
        }

        void IGameConstructElement.ConstructGame(GameContext context)
        {
            particlePool = context.GetService<GUIParticlePoolService>().ImagePool;
            uiCamera = context.GetService<GUICameraService>().Camera;
            worldCamera = WorldCamera.Instance;
            worldViewport = context.GetService<GUIParticleViewportService>().WorldViewport;
        }
    }
}