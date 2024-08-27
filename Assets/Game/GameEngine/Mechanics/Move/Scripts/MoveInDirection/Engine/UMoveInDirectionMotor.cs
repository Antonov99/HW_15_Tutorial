using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Move/Move In Direction Motor")]
    public sealed class UMoveInDirectionMotor : MonoBehaviour, IMoveInDirectionMotor
    {
        private static readonly Vector3 ZERO_DIRECTION = Vector3.zero;

        public event Action OnStartMove;

        public event Action OnStopMove;

        public bool IsMoving
        {
            get { return moveRequired && direction != ZERO_DIRECTION; }
        }

        public bool IsEnabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public Vector3 Direction
        {
            get { return direction; }
        }

        [PropertyOrder(-10)]
        [ReadOnly]
        [ShowInInspector]
        private bool moveRequired;

        [PropertyOrder(-9)]
        [ReadOnly]
        [ShowInInspector]
        private bool finishMove;

        [PropertyOrder(-8)]
        [ReadOnly]
        [ShowInInspector]
        private Vector3 direction;

        [Space]
        [SerializeField]
        private UpdateMode updateMode;

        [Space]
        [SerializeField]
        private UMoveInDirecitonCondition[] preconditions;

        [SerializeField]
        private UMoveInDirectionAction[] startActions;

        [SerializeField]
        private UMoveInDirectionAction[] stopActions;

        private void FixedUpdate()
        {
            if (updateMode == UpdateMode.FIXED_UPDATE)
            {
                UpdateMove();
            }
        }

        private void Update()
        {
            if (updateMode == UpdateMode.UPDATE)
            {
                UpdateMove();
            }
        }

        public bool CanMove(Vector3 direction)
        {
            for (int i = 0, count = preconditions.Length; i < count; i++)
            {
                var condition = preconditions[i];
                if (!condition.IsTrue(direction))
                {
                    return false;
                }
            }

            return true;
        }

        public void RequestMove(Vector3 direction)
        {
            if (!CanMove(direction))
            {
                return;
            }

            this.direction = direction;
            finishMove = false;

            if (!moveRequired)
            {
                moveRequired = true;
                StartMove();
            }
        }

        public void Interrupt()
        {
            if (!moveRequired)
            {
                return;
            }

            finishMove = false;
            moveRequired = false;
            StopMove();
        }

        private void StartMove()
        {
            for (int i = 0, count = startActions.Length; i < count; i++)
            {
                var action = startActions[i];
                action.Do(direction);
            }

            OnStartMove?.Invoke();
        }

        private void UpdateMove()
        {
            if (!moveRequired)
            {
                return;
            }

            if (finishMove)
            {
                moveRequired = false;
                StopMove();
            }

            finishMove = true;
        }

        private void StopMove()
        {
            for (int i = 0, count = stopActions.Length; i < count; i++)
            {
                var action = stopActions[i];
                action.Do(direction);
            }

            OnStopMove?.Invoke();
        }

        private enum UpdateMode
        {
            UPDATE = 0,
            FIXED_UPDATE = 1
        }
    }
}