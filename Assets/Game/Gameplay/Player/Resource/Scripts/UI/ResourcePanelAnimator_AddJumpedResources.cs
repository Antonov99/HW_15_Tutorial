using System.Collections;
using System.Collections.Generic;
using CustomParticles;
using Game.GameEngine;
using Game.GameEngine.GameResources;
using Game.GameEngine.GUI;
using GameSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Gameplay.Player
{
    public sealed class ResourcePanelAnimator_AddJumpedResources : MonoBehaviour,
        IGameConstructElement,
        IGameLateUpdateElement
    {
        [Space]
        [SerializeField]
        private ResourcePanel panel;

        [SerializeField]
        private ResourceInfoCatalog resourceCatalog;

        [Space]
        [SerializeField]
        private int maxParticleCount;

        [Header("Animation")]
        [SerializeField]
        private UIAnimations.JumpSettings[] jumps;

        [SerializeField]
        private UIAnimations.FlySettings settings;

        private ParticlePool<ImageParticle> iconParticlePool;

        private ParticlePool<RectTransform> pointParticlePool;

        private Transform worldViewport;

        private List<RectTransform> pointParticles;

        private List<RectTransform> pointParticlesCache;

        private List<ImageParticle> iconParticles;

        private List<ImageParticle> iconParticlesCache;

        private List<Vector3> startWorldPositions;

        private List<Vector3> startWorldPositionsCache;

        private List<Vector3> startUIPositions;

        private List<Vector3> startUIPositionsCache;

        private Camera worldCamera;

        private Camera uiCamera;
        
        public void PlayIncomeFromWorld(Vector3 startWorldPosition, ResourceType type, int collectAmount)
        {
            StartCoroutine(PlayFromWorldRoutine(startWorldPosition, type, collectAmount));
        }

        private IEnumerator PlayFromWorldRoutine(Vector3 startWorldPosition, ResourceType resourceType, int income)
        {
            if (!panel.IsItemShown(resourceType))
            {
                panel.ShowItem(resourceType);
                yield return new WaitForEndOfFrame(); //Wait for calc UI position    
            }

            var endUIPosition = panel.GetIconCenter(resourceType);
            var info = resourceCatalog.FindResource(resourceType);
            var icon = info.icon;

            var prevValue = panel.GetCurrentValue(resourceType);
            var newValue = prevValue + income;
            var particleIterator = new IntValueIterator(prevValue, newValue, maxParticleCount);
            var particleCount = particleIterator.ParticleCount;

            var currentJumpAngle = 0.0f;
            var deltaJumpAngle = -180 / particleCount;

            for (var i = 0; i < particleCount; i++)
            {
                var jumpAngle = currentJumpAngle + Random.Range(-10.0f, 10.0f);
                var routine = PlayParticle(
                    startWorldPosition,
                    endUIPosition,
                    jumpAngle,
                    particleIterator,
                    resourceType,
                    icon
                );
                StartCoroutine(routine);

                currentJumpAngle += deltaJumpAngle;
            }
        }

        private IEnumerator PlayParticle(
            Vector3 startWorldPosition,
            Vector3 endUIPosiiton,
            float angle,
            IntValueIterator particleIterator,
            ResourceType resourceType,
            Sprite icon
        )
        {
            var iconParticle = iconParticlePool.Get(worldViewport);
            iconParticle.SetIcon(icon);
            iconParticles.Add(iconParticle);

            var pointParticle = pointParticlePool.Get(worldViewport);
            pointParticles.Add(pointParticle);

            startWorldPositions.Add(startWorldPosition);
            var startUIPosition = CameraUtils.FromWorldToUIPosition(
                worldCamera,
                uiCamera,
                startWorldPosition
            );
            startUIPositions.Add(startUIPosition);
            pointParticle.position = startUIPosition;

            yield return UIAnimations.AnimateJumpRoutine(pointParticle, jumps, angle);
            yield return new WaitForSeconds(Random.Range(0.35f, 0.45f));

            startWorldPositions.Remove(startWorldPosition);
            startUIPositions.Remove(startUIPosition);
            pointParticles.Remove(pointParticle);
            iconParticles.Remove(iconParticle);
            pointParticlePool.Release(pointParticle);

            yield return UIAnimations.AnimateFlyRoutine(iconParticle.transform, settings, endUIPosiiton);
            iconParticlePool.Release(iconParticle);

            if (particleIterator.NextValue(out var resourceCount))
            {
                panel.IncrementItem(resourceType, resourceCount);
            }
        }

        void IGameConstructElement.ConstructGame(GameContext context)
        {
            pointParticles = new List<RectTransform>();
            pointParticlesCache = new List<RectTransform>();
            iconParticles = new List<ImageParticle>();
            iconParticlesCache = new List<ImageParticle>();
            startWorldPositions = new List<Vector3>();
            startWorldPositionsCache = new List<Vector3>();
            startUIPositions = new List<Vector3>();
            startUIPositionsCache = new List<Vector3>();

            var guiParticleSystem = context.GetService<GUIParticlePoolService>();
            iconParticlePool = guiParticleSystem.ImagePool;
            pointParticlePool = guiParticleSystem.PointPool;

            uiCamera = context.GetService<GUICameraService>().Camera;
            worldCamera = WorldCamera.Instance;

            worldViewport = context.GetService<GUIParticleViewportService>().WorldViewport;
        }

        void IGameLateUpdateElement.OnLateUpdate(float deltaTime)
        {
            UpdateIconParticles();
        }

        private void UpdateIconParticles()
        {
            pointParticlesCache.Clear();
            pointParticlesCache.AddRange(pointParticles);

            iconParticlesCache.Clear();
            iconParticlesCache.AddRange(iconParticles);

            startWorldPositionsCache.Clear();
            startWorldPositionsCache.AddRange(startWorldPositions);

            startUIPositionsCache.Clear();
            startUIPositionsCache.AddRange(startUIPositions);

            for (int i = 0, count = pointParticlesCache.Count; i < count; i++)
            {
                var point = pointParticlesCache[i];
                var icon = iconParticlesCache[i];
                var iconTransform = icon.transform;
                iconTransform.localScale = point.localScale;

                var startUIPosition = startUIPositionsCache[i];
                var startWorldPosition = startWorldPositionsCache[i];
                iconTransform.position = point.position + EvaluateOffset(startWorldPosition, startUIPosition);
            }
        }

        private Vector3 EvaluateOffset(Vector3 startWorldPosition, Vector3 startUIPosition)
        {
            var uiPosition = CameraUtils.FromWorldToUIPosition(worldCamera, uiCamera, startWorldPosition);
            return uiPosition - startUIPosition;
        }
    }
}