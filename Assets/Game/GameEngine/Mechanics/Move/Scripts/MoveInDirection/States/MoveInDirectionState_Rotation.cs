using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [Serializable]
    public sealed class MoveInDirectionState_Rotation : StateUpdate
    {
        [Space]
        [SerializeField]
        public Mode mode = Mode.INSTANTLY;

        [ShowIf("mode", Mode.SMOOTH)]
        [SerializeField]
        public float rotationSpeed = 45;

        private IMoveInDirectionMotor moveMotor;

        private ITransformEngine transform;

        public void ConstructMotor(IMoveInDirectionMotor moveMotor)
        {
            this.moveMotor = moveMotor;
        }

        public void ConstructTransform(ITransformEngine transform)
        {
            this.transform = transform;
        }

        protected override void Update(float deltaTime)
        {
            if (moveMotor.IsMoving)
            {
                RotateInDirection(deltaTime);
            }
        }

        private void RotateInDirection(float deltaTime)
        {
            var direction = moveMotor.Direction;
            if (mode == Mode.INSTANTLY)
            {
                transform.LookInDirection(direction);
            }
            else if (mode == Mode.SMOOTH)
            {
                transform.RotateTowardsInDirection(direction, rotationSpeed, deltaTime);
            }
        }

        public enum Mode
        {
            INSTANTLY = 0,
            SMOOTH = 1
        }
    }
}