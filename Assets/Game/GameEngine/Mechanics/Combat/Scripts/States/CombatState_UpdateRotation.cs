using System;
using Elementary;

namespace Game.GameEngine.Mechanics
{
    [Serializable]
    public sealed class CombatState_UpdateRotation : StateUpdate
    {
        public Mode mode = Mode.INSTANTLY;

        public float rotationSpeed = 45;

        private IOperator<CombatOperation> combatOperator;

        private ITransformEngine transform;
        
        private IComponent_GetPosition targetComponent;

        public void ConstructOperator(IOperator<CombatOperation> combatOperator)
        {
            this.combatOperator = combatOperator;
        }

        public void ConstructTransform(ITransformEngine transform)
        {
            this.transform = transform;
        }


        public override void Enter()
        {
            targetComponent = combatOperator
                .Current
                .targetEntity
                .Get<IComponent_GetPosition>();
                
            base.Enter();
        }

        protected override void Update(float deltaTime)
        {
            if (combatOperator.IsActive)
            {
                RotateInDirection(deltaTime);
            }
        }

        private void RotateInDirection(float deltaTime)
        {
            var targetPosition = targetComponent.Position;
            if (mode == Mode.INSTANTLY)
            {
                transform.LookAtPosition(targetPosition);
            }
            else if (mode == Mode.SMOOTH)
            {
                transform.RotateTowardsAtPosition(targetPosition, rotationSpeed, deltaTime);
            }
        }

        public enum Mode
        {
            INSTANTLY = 0,
            SMOOTH = 1
        }
    }
}