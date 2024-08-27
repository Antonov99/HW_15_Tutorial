using System;
using Game.GameEngine.Mechanics.Move;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Move/Move To Position Motor")]
    public sealed class UMoveToPositionMotor : MonoBehaviour, IMoveToPositionMotor
    {
        public event Action<Vector3> OnMoveStarted;

        public event Action<Vector3> OnMoveStopped;

        [ShowInInspector, ReadOnly, PropertyOrder(-10), PropertySpace]
        public bool IsMove { get; private set; }

        [ShowInInspector, ReadOnly, PropertyOrder(-9)]
        public Vector3 TargetPosition { get; private set; }

        [SerializeField]
        private UMoveToPositionCondition[] preconditions;

        [SerializeField]
        private UMoveToPositionAction[] startActions;

        [SerializeField]
        private UMoveToPositionAction[] stopActions;

        [Title("Methods")]
        [Button]
        public bool CanStartMove(Vector3 operation)
        {
            if (IsMove)
            {
                return false;
            }

            for (int i = 0, count = preconditions.Length; i < count; i++)
            {
                var condition = preconditions[i];
                if (!condition.IsTrue(operation))
                {
                    return false;
                }
            }

            return true;
        }

        [Button]
        public void StartMove(Vector3 operation)
        {
            if (!CanStartMove(operation))
            {
                return;
            }

            for (int i = 0, count = startActions.Length; i < count; i++)
            {
                var action = startActions[i];
                action.Do(operation);
            }

            TargetPosition = operation;
            IsMove = true;
            OnMoveStarted?.Invoke(operation);
        }

        [Button]
        public void StopMove()
        {
            if (!IsMove)
            {
                return;
            }

            var operation = TargetPosition;
            for (int i = 0, count = stopActions.Length; i < count; i++)
            {
                var action = stopActions[i];
                action.Do(operation);
            }

            IsMove = false;
            TargetPosition = default;
            OnMoveStopped?.Invoke(operation);
        }
    }
}