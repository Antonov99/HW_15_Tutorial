using System;
using UnityEngine;

namespace Entities
{
    [CreateAssetMenu(
        fileName = "ConditionGroup",
        menuName = "Entities/New Condition \'Group\'"
    )]
    public sealed class ScriptableEntityConditionGroup : ScriptableEntityCondition
    {
        [SerializeField]
        private Mode mode;

        [SerializeReference]
        private IEntityCondition[] conditions;

        public override bool IsTrue(IEntity entity)
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

        [Serializable]
        private enum Mode
        {
            AND,
            OR
        }
    }
}