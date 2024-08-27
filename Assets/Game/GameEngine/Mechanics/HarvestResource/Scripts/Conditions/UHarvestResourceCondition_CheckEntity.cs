using Entities;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Harvest Resource/Harvest Resource Condition «Check Entity»")]
    public sealed class UHarvestResourceCondition_CheckEntity : UHarvestResourceCondition
    {
        [SerializeField]
        public ScriptableEntityCondition[] conditions;

        public override bool IsTrue(HarvestResourceOperation operation)
        {
            var targetEntity = operation.targetResource;
            for (int i = 0, count = conditions.Length; i < count; i++)
            {
                var condition = conditions[i];
                if (!condition.IsTrue(targetEntity))
                {
                    return false;
                }
            }

            return true;
        }
    }
}