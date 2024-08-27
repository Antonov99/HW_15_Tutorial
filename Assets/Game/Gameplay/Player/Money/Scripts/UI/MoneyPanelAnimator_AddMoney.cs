using System.Collections;
using CustomParticles;
using Game.GameEngine;
using Game.GameEngine.GUI;
using GameSystem;
using UnityEngine;

namespace Game.Gameplay.Player
{
    public sealed class MoneyPanelAnimator_AddMoney : MonoBehaviour, IGameConstructElement
    {
        [Space]
        [SerializeField]
        private MoneyPanel panel;

        [SerializeField]
        private Sprite moneyIcon;

        [Space]
        [SerializeField]
        private int maxParticleCount;

        [SerializeField]
        private float emissonPeriod;

        [SerializeField]
        private UIAnimations.FlySettings settings;

        private ParticlePool<ImageParticle> particlePool;

        private Transform worldViewport;

        private Transform overlayViewport;

        private Camera worldCamera;

        private Camera uiCamera;

        public void PlayIncomeFromUI(Vector3 startUIPosition, int income)
        {
            StartCoroutine(PlayFromUIRoutine(startUIPosition, income));
        }

        private IEnumerator PlayFromUIRoutine(Vector3 startUIPosition, int income)
        {
            var prevValue = panel.Money;
            var newValue = prevValue + income;
            var particleIterator = new IntValueIterator(prevValue, newValue, maxParticleCount);

            var endUIPosition = panel.GetIconPosition();
            var emissionPeriod = new WaitForSeconds(emissonPeriod);

            for (int i = 0, count = particleIterator.ParticleCount; i < count; i++)
            {
                StartCoroutine(PlayParticleFromUI(startUIPosition, endUIPosition, particleIterator));
                yield return emissionPeriod;
            }
        }

        private IEnumerator PlayParticleFromUI(Vector3 startUIPosition, Vector3 endUIPosition, IntValueIterator particleIterator)
        {
            var particleObject = particlePool.Get(overlayViewport);
            particleObject.SetIcon(moneyIcon);

            var particleTransform = particleObject.transform;
            particleTransform.position = startUIPosition;
            yield return UIAnimations.AnimateFlyRoutine(particleTransform, settings, endUIPosition);
            particlePool.Release(particleObject);

            if (particleIterator.NextValue(out var resourceCount))
            {
                panel.IncrementMoney(resourceCount);
            }
        }

        public void PlayIncomeFromWorld(Vector3 startWorldPosition, int income)
        {
            StartCoroutine(PlayFromWorldRoutine(startWorldPosition, income));
        }

        private IEnumerator PlayFromWorldRoutine(Vector3 startWorldPosition, int income)
        {
            var prevValue = panel.Money;
            var newValue = prevValue + income;
            var particleIterator = new IntValueIterator(prevValue, newValue, maxParticleCount);

            var endUIPosition = panel.GetIconPosition();
            var emissionPeriod = new WaitForSeconds(emissonPeriod);

            for (int i = 0, count = particleIterator.ParticleCount; i < count; i++)
            {
                StartCoroutine(PlayParticleFromWorld(startWorldPosition, endUIPosition, particleIterator));
                yield return emissionPeriod;
            }
        }

        private IEnumerator PlayParticleFromWorld(
            Vector3 startWorldPosition,
            Vector3 endUIPosiiton,
            IntValueIterator particleIterator
        )
        {
            var particleObject = particlePool.Get(worldViewport);
            particleObject.SetIcon(moneyIcon);

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
                panel.IncrementMoney(resourceCount);
            }
        }

        void IGameConstructElement.ConstructGame(GameContext context)
        {
            particlePool = context.GetService<GUIParticlePoolService>().ImagePool;
            uiCamera = context.GetService<GUICameraService>().Camera;
            worldCamera = WorldCamera.Instance;
            
            var viewportService = context.GetService<GUIParticleViewportService>();
            worldViewport = viewportService.WorldViewport;
            overlayViewport = viewportService.OverlayViewport;
        }
    }
}