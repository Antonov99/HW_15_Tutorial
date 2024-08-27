using Elementary;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Move/Move In Direction Mechanics")]
    public sealed class UMoveInDirectionMechanics : MonoBehaviour
    {
        [SerializeField]
        private UMoveInDirectionMotor moveEngine;

        [SerializeField]
        private UTransformEngine transformEngine;

        [SerializeField]
        private FloatAdapter moveSpeed;

        private void FixedUpdate()
        {
            if (moveEngine.IsMoving)
            {
                MoveTransform(moveEngine.Direction);
            }
        }

        private void MoveTransform(Vector3 direction)
        {
            var velocity = direction * (moveSpeed.Current * Time.fixedDeltaTime);
            transformEngine.MovePosition(velocity);
            transformEngine.LookInDirection(direction);
        }
    }
}