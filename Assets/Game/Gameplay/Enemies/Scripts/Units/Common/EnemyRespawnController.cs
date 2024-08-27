using System;
using Elementary;
using Entities;
using Game.GameEngine.Mechanics;
using GameSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Gameplay.Enemies
{
    [Serializable]
    public sealed class EnemyRespawnController :
        IGameReadyElement,
        IGameFinishElement
    {
        [ShowInInspector, ReadOnly]
        private IEntity unit;

        [ShowInInspector, ReadOnly]
        private IEntity ai;

        [ShowInInspector, ReadOnly]
        private Timer timer = new();

        [ShowInInspector, ReadOnly]
        private Transform respawnPoint;

        public void Construct(IEntity unit, IEntity ai, float respawnTime, Transform respawnPoint)
        {
            this.unit = unit;
            this.ai = ai;
            timer.Duration = respawnTime;
            this.respawnPoint = respawnPoint;
        }

        void IGameReadyElement.ReadyGame()
        {
            unit.Get<IComponent_OnDestroyed<DestroyArgs>>().OnDestroyed += OnDestroyed;
            timer.OnFinished += OnTimerFinished;
        }

        void IGameFinishElement.FinishGame()
        {
            unit.Get<IComponent_OnDestroyed<DestroyArgs>>().OnDestroyed -= OnDestroyed;
            timer.OnFinished -= OnTimerFinished;
            timer.Stop();
        }

        private void OnDestroyed(DestroyArgs destroyArgs)
        {
            DisableAI();
            StartTimer();
        }

        private void DisableAI()
        {
            ai.Get<IComponent_Enable>().SetEnable(false);
        }

        private void StartTimer()
        {
            timer.Stop();
            timer.ResetTime();
            timer.Play();
        }

        private void OnTimerFinished()
        {
            RespawnEntity();
        }

        private void RespawnEntity()
        {
            ResetPosition();
            ResetRotation();
            DoRespawn();
            EnableAI();
        }

        private void ResetPosition()
        {
            var positionComponent = unit.Get<IComponent_SetPosition>();
            positionComponent.SetPosition(respawnPoint.position);
        }

        private void ResetRotation()
        {
            var rotationComponent = unit.Get<IComponent_SetRotation>();
            rotationComponent.SetRotation(respawnPoint.rotation);
        }

        private void DoRespawn()
        {
            unit.Get<IComponent_Respawn>().Respawn();
        }
        
        private void EnableAI()
        {
            ai.Get<IComponent_Enable>().SetEnable(true);
        }
    }
}