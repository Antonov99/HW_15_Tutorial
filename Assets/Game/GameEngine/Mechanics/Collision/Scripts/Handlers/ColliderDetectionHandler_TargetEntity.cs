using System;
using Elementary;
using Entities;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [Serializable]
    public abstract class ColliderDetectionHandler_TargetEntity : IColliderDetectionHandler
    {
        [SerializeField]
        public ScriptableEntityCondition[] conditions;
        
        public void OnCollidersUpdated(Collider[] buffer, int size)
        {
            var targetFound = SelectTarget(buffer, size, out var target);
            ProcessTarget(targetFound, target);
        }

        protected abstract void ProcessTarget(bool targetFound, IEntity target);

        protected virtual bool SelectTarget(Collider[] buffer, int size, out IEntity target)
        {
            for (var i = 0; i < size; i++)
            {
                var collider = buffer[i];
                if (collider.TryGetComponent(out IEntity entity) && MatchesEntity(entity))
                {
                    target = entity;
                    return true;
                }
            }

            target = null;
            return false;
        }

        protected virtual bool MatchesEntity(IEntity entity)
        {
            for (int i = 0, count = conditions.Length; i < count; i++)
            {
                var condition = conditions[i];
                if (!condition.IsTrue(entity))
                {
                    return false;
                }
            }

            return true;
        }
    }
}