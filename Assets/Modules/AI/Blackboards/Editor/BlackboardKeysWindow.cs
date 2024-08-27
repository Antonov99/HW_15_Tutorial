#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace AI.Blackboards.UnityEditor
{
    public sealed class BlackboardKeysWindow : EditorWindow
    {
        private SerializedProperty names;

        private SerializedObject serializedObject;

        private Vector2 scrollPosition;

        private void Awake()
        {
            var config = BlackboardKeysConfig.EditorInstance;
            serializedObject = new SerializedObject(config);
            names = serializedObject.FindProperty(nameof(names));

            DrawTitle();
        }

        private void DrawTitle()
        {
            titleContent = new GUIContent("Blackboard Keys");
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

            if (names != null)
            {
                EditorGUILayout.PropertyField(names, includeChildren: true);
            }


            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            if (serializedObject != null)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
#endif