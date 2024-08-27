using Elementary;
using Game.GameEngine;
using Game.GameEngine.Mechanics;
using UnityEngine;

namespace Game.Gameplay.Hero
{
    [AddComponentMenu("GameEngine/Mechanics/Move/Move In Direction State «Surface Position»")]
    public sealed class UMoveInDirectionState_SurfacePosition : MonoState
    {
        [SerializeField]
        private UMoveInDirectionMotor moveEngine;

        [SerializeField]
        private UTransformEngine transformEngine;

        [SerializeField]
        private WalkableSurfaceVariable surfaceHolder;

        [SerializeField]
        private FloatAdapter speed;

        [SerializeField]
        private bool surfaceEnabled = true;

        private bool isEntered;

        public override void Enter()
        {
            isEntered = true;
        }

        public override void Exit()
        {
            isEntered = false;
        }

        private void FixedUpdate()
        {
            if (isEntered)
            {
                MoveInDirection();
            }
        }

        private void MoveInDirection()
        {
            var velocity = moveEngine.Direction * (speed.Current * Time.fixedDeltaTime);
            if (surfaceHolder.IsSurfaceExists && surfaceEnabled)
            {
                MoveBySurface(velocity);
            }
            else
            {
                transformEngine.MovePosition(velocity);
            }
        }

        private void MoveBySurface(Vector3 velocity)
        {
            var nextPosition = transformEngine.WorldPosition + velocity;
            var surface = surfaceHolder.Surface;
            if (surface.IsAvailablePosition(nextPosition))
            {
                transformEngine.SetPosiiton(nextPosition);
            }
            else if (surface.FindAvailablePosition(nextPosition, out var clampedPosition))
            {
                transformEngine.SetPosiiton(clampedPosition);
            }
        }
    }
}