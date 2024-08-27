using UnityEngine;

namespace Polygons
{
    public sealed class MonoPolygon : MonoBehaviour
    {
        private Polygon polygon;

        private void Awake()
        {
            Init();
        }

        public bool IsPointInside(Vector3 position)
        {
            var point2D = ConvertToPoint(position);
            return polygon.IsPointInside(point2D);
        }

        public bool ClampPosition(Vector3 position, out float distance, out Vector3 clampedPosition)
        {
            var point2D = ConvertToPoint(position);
            if (polygon.ClampPosition(point2D, out distance, out var clampedPoint2D))
            {
                clampedPosition = ConventToPosition(clampedPoint2D);
                return true;
            }

            clampedPosition = Vector3.zero;
            return false;
        }

        public Vector3[] GetAllPoints()
        {
            var count = polygon.Length;
            var result = new Vector3[count];
            for (var i = 0; i < count; i++)
            {
                var point2D = polygon.GetPoint(i);
                result[i] = new Vector3(point2D.x, 0.0f, point2D.y);
            }
            
            return result;
        }

        private void Init()
        {
            var count = transform.childCount;
            var points = new Vector2[count];

            for (var i = 0; i < count; i++)
            {
                var child = transform.GetChild(i);
                var worldPosition = child.position;
                points[i] = ConvertToPoint(worldPosition);
            }

            polygon = new Polygon(points);
        }

        private Vector2 ConvertToPoint(Vector3 position)
        {
            return new Vector2(position.x, position.z);
        }

        private Vector3 ConventToPosition(Vector2 point)
        {
            return new Vector3(point.x, 0.0f, point.y);
        }

#if UNITY_EDITOR
        [Space, SerializeField]
        private bool drawGizmos;

        [SerializeField]
        private MonoPolygonDrawer drawer = new();

        private void OnDrawGizmos()
        {
            if (gameObject.activeInHierarchy && drawGizmos)
            {
                Init();
                drawer.DrawPolygon(polygon);
            }
        }

        private void OnValidate()
        {
            Init();
        }
#endif
    }
}