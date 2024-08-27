#if UNITY_EDITOR
using AI.Waypoints;
using UnityEditor;

namespace AI.UnityEditor
{
    [CustomEditor(typeof(WaypointsPath))]
    public sealed class WaypointsPathEditor : Editor
    {
        private SerializedProperty drawGizmos;

        private SerializedProperty loop;

        private SerializedProperty color;

        private void Awake()
        {
            drawGizmos = serializedObject.FindProperty(nameof(drawGizmos));
            loop = serializedObject.FindProperty(nameof(loop));
            color = serializedObject.FindProperty(nameof(color));
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space(4.0f);
            
            drawGizmos.boolValue = EditorGUILayout.BeginToggleGroup("Draw Gizmos", drawGizmos.boolValue);
            EditorGUILayout.PropertyField(loop);
            EditorGUILayout.PropertyField(color);
            EditorGUILayout.EndToggleGroup();
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif