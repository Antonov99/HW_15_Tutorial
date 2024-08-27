using Elementary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Move/Move In Direction State «Rotation»")]
    public sealed class UMoveInDirectionState_Rotation : MonoState
    {
        [SerializeField]
        private UMoveInDirectionMotor moveEngine;
        
        [SerializeField]
        private UTransformEngine transformEngine;

        [Space]
        [SerializeField]
        private Mode mode = Mode.INSTANTLY;

        [ShowIf("mode", Mode.SMOOTH)]
        [SerializeField]
        private FloatAdapter speed; //45

        private void Awake()
        {
            enabled = false;
        }

        private void Update()
        {
            if (moveEngine.IsMoving)
            {
                RotateInDirection();
            }
        }

        public override void Enter()
        {
            enabled = true;
        }

        public override void Exit()
        {
            enabled = false;
        }

        private void RotateInDirection()
        {
            var direction = moveEngine.Direction;
            if (mode == Mode.INSTANTLY)
            {
                transformEngine.LookInDirection(direction);
            }
            else if (mode == Mode.SMOOTH)
            {
                transformEngine.RotateTowardsInDirection(direction, speed.Current, Time.deltaTime);
            }
        }

        private enum Mode
        {
            INSTANTLY = 0,
            SMOOTH = 1
        }
    }
}