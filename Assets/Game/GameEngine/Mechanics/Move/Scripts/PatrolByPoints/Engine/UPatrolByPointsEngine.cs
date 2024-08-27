using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Move/Patrol By Points Engine")]
    public sealed class UPatrolByPointsEngine : MonoBehaviour, IPatrolByPointsEngine
    {
        public event Action<PatrolByPointsOperation> OnPatrolStarted;

        public event Action<PatrolByPointsOperation> OnPatrolStopped;

        [ShowInInspector, ReadOnly, PropertyOrder(-10), PropertySpace]
        public bool IsPatrol { get; private set; }

        [ShowInInspector, ReadOnly, PropertyOrder(-9)]
        public PatrolByPointsOperation CurrentOperation { get; private set; }

        [SerializeField]
        private UPatrolByPointsCondition[] preconditions;

        [SerializeField]
        private UPatrolByPointsAction[] startActions;

        [SerializeField]
        private UPatrolByPointsAction[] stopActions;

        [Title("Methods")]
        [Button]
        public bool CanStartPatrol(PatrolByPointsOperation operation)
        {
            if (IsPatrol)
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
        public void StartPatrol(PatrolByPointsOperation operation)
        {
            if (!CanStartPatrol(operation))
            {
                return;
            }

            for (int i = 0, count = startActions.Length; i < count; i++)
            {
                var action = startActions[i];
                action.Do(operation);
            }

            CurrentOperation = operation;
            IsPatrol = true;
            OnPatrolStarted?.Invoke(operation);
        }

        [Button]
        public void StopPatrol()
        {
            if (!IsPatrol)
            {
                return;
            }

            var operation = CurrentOperation;
            for (int i = 0, count = stopActions.Length; i < count; i++)
            {
                var action = stopActions[i];
                action.Do(operation);
            }

            IsPatrol = false;
            CurrentOperation = default;
            OnPatrolStopped?.Invoke(operation);
        }
    }
}