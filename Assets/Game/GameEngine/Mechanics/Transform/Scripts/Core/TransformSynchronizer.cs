using System;
using Declarative;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [Serializable]
    public sealed class TransformSynchronizer :
        IUpdateListener,
        IFixedUpdateListener,
        ILateUpdateListener
    {
        public Mode mode;

        public bool isEnabled;

        [Space]
        public Transform sourcePosition;

        public Transform[] syncPosiitions;

        [Space]
        public Transform sourceRotation;
        
        public Transform[] syncRotations;

        void IUpdateListener.Update(float deltaTime)
        {
            if (isEnabled && mode == Mode.UPDATE)
            {
                SyncPositions();
                SyncRotations();
            }
        }

        void IFixedUpdateListener.FixedUpdate(float deltaTime)
        {
            if (isEnabled && mode == Mode.FIXED_UPDATE)
            {
                SyncPositions();
                SyncRotations();
            }
        }

        void ILateUpdateListener.LateUpdate(float deltaTime)
        {
            if (isEnabled && mode == Mode.LATE_UPDATE)
            {
                SyncPositions();
                SyncRotations();
            }
        }

        private void SyncPositions()
        {
            
            var position = sourcePosition.position;
            for (int i = 0, count = syncPosiitions.Length; i < count; i++)
            {
                var targetTransform = syncPosiitions[i];
                targetTransform.position = position;
            }
        }

        private void SyncRotations()
        {
            var rotation = sourceRotation.rotation;
            for (int i = 0, count = syncRotations.Length; i < count; i++)
            {
                var targetTransform = syncRotations[i];
                targetTransform.rotation = rotation;
            }
        }

        public enum Mode
        {
            UPDATE = 0,
            FIXED_UPDATE = 1,
            LATE_UPDATE = 2
        }
    }
}