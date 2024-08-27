using System;
using UnityEngine;

namespace AI.BTree
{
    [AddComponentMenu(Extensions.MENU_PATH + "Condition «Group»")]
    public sealed class UnityBehaviourConditionGroup : UnityBehaviourCondition
    {
        [SerializeField]
        private Mode mode;

        [SerializeField]
        private UnityBehaviourCondition[] conditions;
        
        public override bool IsTrue()
        {
            return mode switch
            {
                Mode.AND => All(),
                Mode.OR => Any(),
                _ => throw new Exception($"Mode is undefined {mode}")
            };
        }

        private bool All()
        {
            for (int i = 0, count = conditions.Length; i < count; i++)
            {
                var condition = conditions[i];
                if (!condition.IsTrue())
                {
                    return false;
                }
            }

            return true;
        }

        private bool Any()
        {
            for (int i = 0, count = conditions.Length; i < count; i++)
            {
                var condition = conditions[i];
                if (condition.IsTrue())
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