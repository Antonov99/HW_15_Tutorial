#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace GameSystem.UnityEditor
{
    [CustomEditor(typeof(GameServiceGroup))]
    public sealed class GameServiceGroupEditor : Editor
    {
        private GameServiceGroup serviceGroup;

        private SerializedProperty gameServices;

        private DragAndDropDrawler dragAndDropDrawler;

        private void OnEnable()
        {
            serviceGroup = (GameServiceGroup) target;
            gameServices = serializedObject.FindProperty(nameof(gameServices));
            dragAndDropDrawler = DragAndDropDrawler.CreateForServices(OnDragAndDrop);
        }

        private void OnDisable()
        {
            dragAndDropDrawler.OnDragAndDrop -= OnDragAndDrop;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawGameServices();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawGameServices()
        {
            EditorGUILayout.PropertyField(gameServices, includeChildren: true);
            EditorGUILayout.Space(8);
            dragAndDropDrawler.Draw();
        }
        
        private void OnDragAndDrop(Object draggedObject)
        {
            if (draggedObject is GameObject gameObject)
            {
                AddByGameObject(gameObject);
                EditorUtility.SetDirty(serviceGroup);
            }

            if (draggedObject is MonoBehaviour monoBehaviour)
            {
                AddByMonoBehaviour(monoBehaviour);
                EditorUtility.SetDirty(serviceGroup);
            }
        }

        private void AddByGameObject(GameObject gameObject)
        {
            var monoBehaviours = gameObject.GetComponents<MonoBehaviour>();
            foreach (var monoBehaviour in monoBehaviours)
            {
                AddByMonoBehaviour(monoBehaviour);
            }
        }

        private void AddByMonoBehaviour(MonoBehaviour monoBehaviour)
        {
            if (ReferenceEquals(monoBehaviour, serviceGroup))
            {
                return;
            }
            
            if (monoBehaviour is IGameServiceGroup)
            {
                serviceGroup.Editor_AddService(monoBehaviour);
                return;
            }

            if (monoBehaviour is not IGameElementGroup)
            {
                serviceGroup.Editor_AddService(monoBehaviour);
            }
        }
    }
}
#endif