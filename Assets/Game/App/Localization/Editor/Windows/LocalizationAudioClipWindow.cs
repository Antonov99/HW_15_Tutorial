#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Game.Localization.UnityEditor
{
    public sealed class LocalizationAudioClipWindow : EditorWindow
    {
        private SerializedObject serializedObject;

        private SerializedProperty entities;

        private Vector2 scrollPosition;

        private void OnEnable()
        {
            titleContent = new GUIContent("Localization Audio Clips");
            serializedObject = new SerializedObject(Configs.AudioClipConfig);
            entities = serializedObject.FindProperty("entities");
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(8);

            EditorGUILayout.BeginVertical();
            scrollPosition = EditorGUILayout.BeginScrollView(
                scrollPosition,
                GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true)
            );
            
            EditorGUILayout.PropertyField(entities, includeChildren: true);
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
    }
}
#endif