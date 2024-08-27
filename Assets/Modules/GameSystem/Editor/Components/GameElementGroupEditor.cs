#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameSystem.UnityEditor
{
    [CustomEditor(typeof(GameElementGroup))]
    public sealed class GameElementGroupEditor : Editor
    {
        private GameElementGroup elementGroup;

        private SerializedProperty gameElements;

        private DragAndDropDrawler dragAndDropDrawler;

        private void OnEnable()
        {
            elementGroup = (GameElementGroup) target;
            gameElements = serializedObject.FindProperty(nameof(gameElements));
            dragAndDropDrawler = DragAndDropDrawler.CreateForElements(OnDragAndDrop);
        }

        private void OnDisable()
        {
            dragAndDropDrawler.OnDragAndDrop -= OnDragAndDrop;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawGameElements();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawGameElements()
        {
            EditorGUILayout.PropertyField(gameElements, includeChildren: true);
            EditorGUILayout.Space(8);
            dragAndDropDrawler.Draw();
        }

        private void OnDragAndDrop(Object draggedObject)
        {
            if (draggedObject is GameObject gameObject)
            {
                AddByGameObject(gameObject);
                EditorUtility.SetDirty(elementGroup);
            }

            if (draggedObject is IGameElement gameElement)
            {
                AddByGameElement(gameElement);
                EditorUtility.SetDirty(elementGroup);
            }
        }

        private void AddByGameObject(GameObject gameObject)
        {
            var gameElements = gameObject.GetComponents<IGameElement>();
            foreach (var element in gameElements)
            {
                AddByGameElement(element);
            }
        }

        private void AddByGameElement(IGameElement element)
        {
            if (!ReferenceEquals(element, elementGroup))
            {
                elementGroup.Editor_AddElement(element);
            }
        }
    }
}
#endif