#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Services.UnityEditor
{
    [CustomEditor(typeof(ServicePack))]
    public sealed class ServicePackEditor : Editor
    {
        private SerializedProperty editorMode;

        private SerializedProperty releaseScripts;

        private SerializedProperty editorScripts;

        private void OnEnable()
        {
            editorMode = serializedObject.FindProperty(nameof(editorMode));
            releaseScripts = serializedObject.FindProperty(nameof(releaseScripts));
            editorScripts = serializedObject.FindProperty(nameof(editorScripts));
        }

        public override void OnInspectorGUI()
        {
            if (editorMode.boolValue)
            {
                GUI.enabled = false;
            }

            EditorGUILayout.PropertyField(releaseScripts, includeChildren: true);
            GUI.enabled = true;

            EditorGUILayout.Space(4.0f);
            editorMode.boolValue = EditorGUILayout.BeginToggleGroup("Editor Mode", editorMode.boolValue);

            EditorGUILayout.Space(4.0f);
            EditorGUILayout.PropertyField(editorScripts, includeChildren: true);
            EditorGUILayout.EndToggleGroup();

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
#endif