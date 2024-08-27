using System;
using Elementary;

namespace Game.GameEngine.Mechanics
{
    [Serializable]
    public sealed class MoveInDirectionState_Position : StateFixedUpdate
    {
        private IMoveInDirectionMotor motor;

        private ITransformEngine transform;

        private IValue<float> speed;

        public void ConstructMotor(IMoveInDirectionMotor motor)
        {
            this.motor = motor;
        }

        public void ConstructTransform(ITransformEngine transform)
        {
            this.transform = transform;
        }

        public void ConstructSpeed(IValue<float> speed)
        {
            this.speed = speed;
        }

        protected override void FixedUpdate(float deltaTime)
        {
            if (motor.IsMoving)
            {
                MoveInDirection(deltaTime);
            }
        }

        private void MoveInDirection(float deltaTime)
        {
            var velocity = motor.Direction * (speed.Current * deltaTime);
            transform.MovePosition(velocity);
        }
    }
}