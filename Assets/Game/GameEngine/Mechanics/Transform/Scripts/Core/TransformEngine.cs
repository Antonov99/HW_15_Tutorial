using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [Serializable]
    public sealed class TransformEngine : ITransformEngine
    {
        private static readonly Vector3 UP = Vector3.up;

        [ShowInInspector, ReadOnly, PropertyOrder(-10), PropertySpace]
        public Vector3 WorldPosition
        {
            get { return GetWorldPosition(); }
        }

        [ShowInInspector, ReadOnly, PropertyOrder(-9)]
        public Quaternion WorldRotation
        {
            get { return GetWorldRotation(); }
        }

        [Space]
        [SerializeField]
        private Transform sourcePositionTransform;

        [SerializeField]
        private Transform[] movingTransforms;

        [Space]
        [SerializeField]
        private Transform sourceRotationTransform;

        [SerializeField]
        private Transform[] rotatingTransforms;

        [Title("Methods")]
        [Button]
        [GUIColor(0, 1, 0)]
        public void SetPosiiton(Vector3 position)
        {
            position.y = 0;
            for (int i = 0, count = movingTransforms.Length; i < count; i++)
            {
                var transform = movingTransforms[i];
                transform.position = position;
            }
        }

        [Button]
        [GUIColor(0, 1, 0)]
        public void MovePosition(Vector3 vector)
        {
            var newPosition = sourcePositionTransform.position + vector;
            SetPosiiton(newPosition);
        }

        [Button]
        [GUIColor(0, 1, 0)]
        public bool IsDistanceReached(Vector3 targetPosition, float minDistance)
        {
            var distanceVector = sourcePositionTransform.position - targetPosition;
            distanceVector.y = 0.0f;
            return distanceVector.sqrMagnitude <= minDistance * minDistance;
        }

        [Button]
        [GUIColor(0, 1, 0)]
        public void SetRotation(Quaternion rotation)
        {
            var eulerAngles = rotation.eulerAngles;
            eulerAngles.x = 0;
            eulerAngles.z = 0;

            for (int i = 0, count = rotatingTransforms.Length; i < count; i++)
            {
                var transform = rotatingTransforms[i];
                transform.eulerAngles = eulerAngles;
            }
        }

        [Button]
        [GUIColor(0, 1, 0)]
        public void LookAtPosition(Vector3 targetPosition)
        {
            var distanceVector = targetPosition - sourcePositionTransform.position;
            var direction = distanceVector.normalized;
            LookInDirection(direction);
        }

        [Button]
        [GUIColor(0, 1, 0)]
        public void LookInDirection(Vector3 direction)
        {
            var newRotation = Quaternion.LookRotation(direction, UP);
            SetRotation(newRotation);
        }

        [Button]
        [GUIColor(0, 1, 0)]
        public void RotateTowardsAtPosition(Vector3 targetPosition, float speed, float deltaTime)
        {
            var distanceVector = targetPosition - sourcePositionTransform.position;
            var direction = distanceVector.normalized;
            RotateTowardsInDirection(direction, speed, deltaTime);
        }

        [Button]
        [GUIColor(0, 1, 0)]
        public void RotateTowardsInDirection(Vector3 direction, float speed, float deltaTime)
        {
            var currentRotation = sourceRotationTransform.rotation;
            var targetRotation = Quaternion.LookRotation(direction, UP);
            var newRotation = Quaternion.Slerp(currentRotation, targetRotation, speed * deltaTime);
            SetRotation(newRotation);
        }

        private Vector3 GetWorldPosition()
        {
            if (sourcePositionTransform != null)
            {
                return sourcePositionTransform.position;
            }

            return Vector3.zero;
        }

        private Quaternion GetWorldRotation()
        {
            if (sourceRotationTransform != null)
            {
                return sourceRotationTransform.rotation;
            }

            return Quaternion.identity;
        }
    }
}