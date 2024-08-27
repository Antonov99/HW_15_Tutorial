#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Game.Localization.UnityEditor
{
    public sealed class LocalizationTextWindow : EditorWindow
    {
        private SerializedObject serializedObject;

        private SerializedProperty spreadsheet;

        private LocalizationTextConfig config;

        private int toolbarIndex;

        private Vector2 scrollPosition;

        private void OnEnable()
        {
            titleContent = new GUIContent("Localization Texts");
            config = Configs.TextConfig;
            serializedObject = new SerializedObject(config);
            spreadsheet = serializedObject.FindProperty(nameof(spreadsheet));
        }

        private void OnGUI()
        {
            var pageNames = GetPageNames();
            if (pageNames.Length <= 0)
            {
                return;
            }

            toolbarIndex = GUILayout.Toolbar(toolbarIndex, pageNames);

            EditorGUILayout.Space(8);
            EditorGUILayout.BeginVertical();
            scrollPosition = EditorGUILayout.BeginScrollView(
                scrollPosition,
                GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true)
            );
            
            var pages = spreadsheet.FindPropertyRelative("pages");
            var page = pages.GetArrayElementAtIndex(toolbarIndex);

            var entities = page.FindPropertyRelative("entities");
            EditorGUILayout.PropertyField(entities, includeChildren: true);
            
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private string[] GetPageNames()
        {
            return config.spreadsheet.pages
                .Select(it => it.name)
                .ToArray();
        }
    }
}
#endif