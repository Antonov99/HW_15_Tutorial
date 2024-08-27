using System;
using Elementary;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [Serializable]
    public sealed class CombatState_ControlTargetDistance : State_CheckDistanceToTarget
    {
        private IOperator<CombatOperation> combatEngine;

        private IComponent_GetPosition targetComponent;

        public void ConstructOperator(IOperator<CombatOperation> combatOperator)
        {
            combatEngine = combatOperator;
        }

        protected override void OnEnter()
        {
            targetComponent = combatEngine
                .Current
                .targetEntity
                .Get<IComponent_GetPosition>();
        }
        
        protected override void ProcessDistance(bool distanceReached)
        {
            if (!distanceReached)
            {
                combatEngine.Stop();
            }
        }

        protected override Vector3 GetTargetPosition()
        {
            return targetComponent.Position;
        }
    }
}