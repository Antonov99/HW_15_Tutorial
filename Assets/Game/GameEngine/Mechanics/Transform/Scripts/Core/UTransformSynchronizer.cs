using System;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Transform/Transform Synchronizer")]
    public sealed class UTransformSynchronizer : MonoBehaviour
    {
        [SerializeField]
        private Mode mode;

        [SerializeField]
        private Settings posititonSettings;

        [SerializeField]
        private Settings rotationSettings;

        private void Update()
        {
            if (mode == Mode.UPDATE)
            {
                SyncPositions();
                SyncRotations();
            }
        }

        private void FixedUpdate()
        {
            if (mode == Mode.FIXED_UPDATE)
            {
                SyncPositions();
                SyncRotations();
            }
        }

        private void LateUpdate()
        {
            if (mode == Mode.LATE_UPDATE)
            {
                SyncPositions();
                SyncRotations();
            }
        }

        private void SyncPositions()
        {
            if (!posititonSettings.enabled)
            {
                return;
            }
        
            var position = posititonSettings.source.position;
            var targets = posititonSettings.targets;
            
            for (int i = 0, count = targets.Length; i < count; i++)
            {
                var targetTransform = targets[i];
                targetTransform.position = position;
            }
        }

        private void SyncRotations()
        {
            if (!rotationSettings.enabled)
            {
                return;
            }
        
            var rotation = rotationSettings.source.rotation;
            var targets = rotationSettings.targets;
            
            for (int i = 0, count = targets.Length; i < count; i++)
            {
                var targetTransform = targets[i];
                targetTransform.rotation = rotation;
            }
        }

        private enum Mode
        {
            UPDATE,
            FIXED_UPDATE,
            LATE_UPDATE
        }

        [Serializable]
        private struct Settings
        {
            [SerializeField]
            public bool enabled;
        
            [SerializeField]
            public Transform source;

            [SerializeField]
            public Transform[] targets;
        } 
    }
}