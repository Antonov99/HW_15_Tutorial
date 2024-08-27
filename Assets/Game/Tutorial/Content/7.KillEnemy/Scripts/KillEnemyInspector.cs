using System;
using Entities;
using Game.GameEngine.Mechanics;
using GameSystem;
using Game.GameEngine;
using Game.Gameplay.Hero;
using UnityEngine;

namespace Game.Tutorial
{
    public sealed class KillEnemyInspector
    {
        private IEntity targetEnemy;
    
        private Action<IEntity> callback;

        public void Inspect(IEntity enemy, Action<IEntity> callback)
        {
            this.callback = callback;
            targetEnemy = enemy;
            targetEnemy.Get<IComponent_OnDestroyed<DestroyArgs>>().OnDestroyed += OnEnemyDestroyed;
        }

        private void OnEnemyDestroyed(DestroyArgs destroyArgs)
        {
            targetEnemy.Get<IComponent_OnDestroyed<DestroyArgs>>().OnDestroyed -= OnEnemyDestroyed;
            var enemy = targetEnemy;
            targetEnemy = null;
            callback?.Invoke(enemy);
        }
    }
}