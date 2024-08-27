using System;
using UnityEngine;

namespace Entities
{
    [Serializable]
    public sealed class EntityConditonGroup : IEntityCondition
    {
        [SerializeReference]
        private IEntityCondition[] conditions;

        [SerializeField]
        private Mode mode;

        public EntityConditonGroup(Mode mode, params IEntityCondition[] conditions)
        {
            this.conditions = conditions;
            this.mode = mode;
        }

        public bool IsTrue(IEntity entity)
        {
            return mode switch
            {
                Mode.AND => All(entity),
                Mode.OR => Any(entity),
                _ => throw new Exception($"Mode is undefined {mode}")
            };
        }

        private bool All(IEntity entity)
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

        private bool Any(IEntity entity)
        {
            for (int i = 0, count = conditions.Length; i < count; i++)
            {
                var condition = conditions[i];
                if (condition.IsTrue(entity))
                {
                    return true;
                }
            }

            return false;
        }

        public enum Mode
        {
            AND,
            OR
        }
    }
}