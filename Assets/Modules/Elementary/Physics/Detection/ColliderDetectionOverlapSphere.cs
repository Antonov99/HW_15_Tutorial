using System;
using UnityEditor;
using UnityEngine;

namespace Elementary
{
    [AddComponentMenu("Elementary/Physics/Collider Detection «Overlap Sphere»")]
    public sealed class ColliderDetectionOverlapSphere : ColliderDetection
    {
        [Space]
        [SerializeField]
        private Transform centerPoint;

        [SerializeField]
        private float radius;
        
        [SerializeField]
        private LayerMask layerMask;

        [SerializeField]
        private QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.UseGlobal;

        private Coroutine coroutine;

        public void SetCenterPoint(Transform centerPoint)
        {
            this.centerPoint = centerPoint;
        }

        protected override int Detect(Collider[] buffer)
        {
            return Physics.OverlapSphereNonAlloc(
                position: centerPoint.position,
                radius: radius,
                results: buffer,
                layerMask: layerMask,
                queryTriggerInteraction: triggerInteraction
            );
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            try
            {
                var prevColor = Handles.color;
                Handles.color = Color.blue;
                Handles.DrawWireDisc(centerPoint.position, Vector3.up, radius, 1.25f);
                Handles.color = prevColor;
            }
            catch (Exception)
            {
                // ignored
            }
        }
#endif
    }
}