using System;
using Declarative;
using Elementary;
using Game.GameEngine.Mechanics;
using UnityEngine;

namespace Game.Gameplay.Dummies
{
    public sealed class DummyModel : DeclarativeModel
    {
        [Section]
        [SerializeField]
        public ScriptableDummy config;

        [Section]
        [SerializeField]
        public Core core;

        [Section]
        [SerializeField]
        private Animations animations;

        [Section]
        [SerializeField]
        private Canvas canvas;

        [Serializable]
        public sealed class Core
        {
            [SerializeField]
            public GameObject collisionLayer;

            [SerializeField]
            public HitPoints hitPointsEngine = new();

            [Space]
            [SerializeField]
            public TakeDamageEngine takeDamageEngine = new();

            [SerializeField]
            public Emitter<DestroyArgs> destroyEmitter = new();

            [Construct]
            private void ConstructHitPoints(ScriptableDummy config)
            {
                var hitPoints = config.hitPoints;
                hitPointsEngine.Max = hitPoints;
                hitPointsEngine.Current = hitPoints;
            }

            [Construct]
            private void ConstructTakeDamage()
            {
                takeDamageEngine.Construct(hitPointsEngine, destroyEmitter);
            }

            [Construct]
            private void ConstructDestroy()
            {
                destroyEmitter.AddListener(_ => collisionLayer.SetActive(false));
            }

            [Construct]
            private void ConstructCollision()
            {
                collisionLayer.SetActive(true);
            }
        }

        [Serializable]
        public sealed class Animations
        {
            [SerializeField]
            private Animator animator;

            [SerializeField]
            private string hitAnimation = "Hit";

            [SerializeField]
            private string destroyAnimation = "Destroy";

            [Construct]
            private void Construct(Core core)
            {
                core.takeDamageEngine.AddListener(_ => animator.Play(hitAnimation, -1, 0));
                core.destroyEmitter.AddListener(_ => animator.Play(destroyAnimation, -1, 0));
            }
        }

        [Serializable]
        private sealed class Canvas
        {
            [SerializeField]
            private HitPointsBar hitPointsView;
            
            private readonly HitPointsBarAdapterV1 hitPointsViewAdapter = new();

            [Construct]
            private void Construct(Core core)
            {
                hitPointsViewAdapter.Construct(core.hitPointsEngine, hitPointsView);
            }
        }
    }
}