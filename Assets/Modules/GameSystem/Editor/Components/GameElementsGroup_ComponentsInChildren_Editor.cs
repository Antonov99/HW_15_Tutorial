#if UNITY_EDITOR
using System.Collections.Generic;
using GameSystem.Extensions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameSystem.UnityEditor
{
    [CustomEditor(typeof(GameElementGroup_ComponentsInChildren))]
    public sealed class GameElementsGroup_ComponentsInChildren_Editor : Editor
    {
        private SerializedProperty includeInactive;

        private bool showGameElements = true;

        private void Awake()
        {
            includeInactive = serializedObject.FindProperty(nameof(includeInactive));
        }

        public override void OnInspectorGUI()
        {
            var target = (MonoBehaviour) this.target;
            if (target.gameObject.activeSelf)
            {
                DrawIncludeField();
                DrawInfo(target);
            }
            else
            {
                EditorGUILayout.HelpBox("Game Element Group is inactive!", MessageType.Warning);
            }
        }

        private void DrawIncludeField()
        {
            EditorGUILayout.Space(4.0f);
            EditorGUILayout.PropertyField(includeInactive);
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawInfo(MonoBehaviour target)
        {
            EditorGUILayout.Space(4.0f);
            GUI.enabled = false;

            showGameElements = EditorGUILayout.Foldout(showGameElements, "Game Elements");
            if (showGameElements)
            {
                DrawGameElements(target);
            }

            GUI.enabled = true;
        }

        private void DrawGameElements(MonoBehaviour target)
        {
            var gameElements = new List<IGameElement>(capacity: 0);
            target.GetComponentsInChildren<IGameElement>(includeInactive.boolValue, gameElements);
            gameElements.Remove((IGameElement) target);

            foreach (var gameElement in gameElements)
            {
                var unityObject = (Object) gameElement;
                EditorGUILayout.ObjectField(obj: unityObject, objType: typeof(IGameElement), allowSceneObjects: true);
            }
        }
    }
}
#endif