using System;
using System.Collections;
using Entities;
using Game.GameEngine.Mechanics;
using GameSystem;
using UnityEngine;

namespace Game.Gameplay.Hero
{
    [Serializable]
    public sealed class RespawnInteractor : IGameInitElement
    {
        private HeroService heroService;

        private MonoBehaviour monoContext;
        
        private IEntity hero;

        [SerializeField]
        private float delay = 0.25f;

        private Transform spawnPoint;

        private Coroutine respawnCoroutine;

        [GameInject]
        public void Construct(HeroService heroService, MonoBehaviour monoContext)
        {
            this.heroService = heroService;
            this.monoContext = monoContext;
        }
        
        void IGameInitElement.InitGame()
        {
            hero = heroService.GetHero();
            ResetPosition();
            ResetRotation();
        }

        public void SetupSpawnPoint(Transform spawnPoint)
        {
            this.spawnPoint = spawnPoint;
        }

        public void StartRespawn()
        {
            if (respawnCoroutine == null)
            {
                respawnCoroutine = monoContext.StartCoroutine(RespawnRoutine());
            }
        }

        private IEnumerator RespawnRoutine()
        {
            yield return new WaitForSeconds(delay);
            ResetPosition();
            ResetRotation();
            InvokeRespawn();

            respawnCoroutine = null;
        }


        private void ResetPosition()
        {
            hero
                .Get<IComponent_SetPosition>()
                .SetPosition(spawnPoint.position);
        }

        private void ResetRotation()
        {
            hero
                .Get<IComponent_SetRotation>()
                .SetRotation(spawnPoint.rotation);
        }

        private void InvokeRespawn()
        {
            hero
                .Get<IComponent_Respawn>()
                .Respawn();
        }
    }
}