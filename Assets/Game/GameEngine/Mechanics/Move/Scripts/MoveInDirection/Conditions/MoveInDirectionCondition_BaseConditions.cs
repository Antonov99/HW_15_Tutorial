using Elementary;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    public sealed class MoveInDirectionCondition_BaseConditions : ICondition<Vector3>
    {
        public ICondition[] conditions;

        public MoveInDirectionCondition_BaseConditions(params ICondition[] conditions)
        {
            this.conditions = conditions;
        }

        public bool IsTrue(Vector3 value)
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
    }
}