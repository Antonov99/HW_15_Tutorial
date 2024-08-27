using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public sealed class MonoEntityConditionGroup : MonoEntityCondition
    {
        [SerializeField]
        private Mode mode;

        [Space]
        [SerializeReference]
        private IEntityCondition[] baseConditions;
        
        [SerializeField]
        private MonoEntityCondition[] monoConditions;

        [SerializeField]
        private ScriptableEntityCondition[] scriptableConditions;
        
        private List<IEntityCondition> conditions;

        private void Awake()
        {
            conditions = new List<IEntityCondition>();
            conditions.AddRange(baseConditions);
            conditions.AddRange(monoConditions);
            conditions.AddRange(scriptableConditions);
        }

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
            for (int i = 0, count = conditions.Count; i < count; i++)
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
            for (int i = 0, count = conditions.Count; i < count; i++)
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