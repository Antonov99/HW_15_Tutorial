using Elementary;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Move/Move In Direction State «Position»")]
    public sealed class UMoveInDirectionState_Position : MonoState
    {
        [SerializeField]
        private UMoveInDirectionMotor moveEngine;

        [SerializeField]
        private UTransformEngine transformEngine;

        [SerializeField]
        private FloatAdapter speed;

        private void Awake()
        {
            enabled = false;
        }

        private void FixedUpdate()
        {
            if (moveEngine.IsMoving)
            {
                MoveInDirection();
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

        private void MoveInDirection()
        {
            var velocity = moveEngine.Direction * (speed.Current * Time.fixedDeltaTime);
            transformEngine.MovePosition(velocity);
        }
    }
}