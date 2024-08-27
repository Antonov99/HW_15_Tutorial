#if UNITY_EDITOR
using GameSystem.Extensions;
using UnityEditor;
using UnityEngine;

namespace GameSystem.UnityEditor
{
    [CustomEditor(typeof(GameElementGroup_ChildTransforms))]
    public sealed class GameElementsGroup_ChildTransforms_Editor : Editor
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
                EditorGUILayout.HelpBox("Game Elements Group is inactive!", MessageType.Warning);
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
            var transform = target.transform;
            if (includeInactive.boolValue)
            {
                foreach (Transform child in transform)
                {
                    if (child.TryGetComponent(out IGameElement gameElement))
                    {
                        var unityObject = (Object) gameElement;
                        EditorGUILayout.ObjectField(obj: unityObject, objType: typeof(IGameElement), allowSceneObjects: true);
                    }
                }    
            }
            else
            {
                foreach (Transform child in transform)
                {
                    if (child.gameObject.activeSelf && child.TryGetComponent(out IGameElement gameElement))
                    {
                        var unityObject = (Object) gameElement;
                        EditorGUILayout.ObjectField(obj: unityObject, objType: typeof(IGameElement), allowSceneObjects: true);
                    }
                }   
            }
        }
    }
}
#endif