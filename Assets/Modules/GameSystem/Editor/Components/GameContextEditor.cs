#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameSystem.UnityEditor
{
    [CustomEditor(typeof(GameContext))]
    public sealed class GameContextEditor : Editor
    {
        private GameContext gameContext;

        private SerializedProperty autoRun;

        private SerializedProperty gameServices;

        private SerializedProperty gameElements;

        private SerializedProperty constructTasks;

        private DragAndDropDrawler dragAndDropServiceDrawler;

        private DragAndDropDrawler dragAndDropElementDrawler;

        private DragAndDropDrawler dragAndDropInitTaskDrawler;

        private void OnEnable()
        {
            gameContext = (GameContext) target;

            autoRun = serializedObject.FindProperty(nameof(autoRun));
            
            gameServices = serializedObject.FindProperty(nameof(gameServices));
            gameElements = serializedObject.FindProperty(nameof(gameElements));
            constructTasks = serializedObject.FindProperty(nameof(constructTasks));

           dragAndDropServiceDrawler = DragAndDropDrawler.CreateForServices(OnDragAndDropService);
           dragAndDropElementDrawler = DragAndDropDrawler.CreateForElements(OnDragAndDropElement);
           dragAndDropInitTaskDrawler = DragAndDropDrawler.CreateForConstructTasks(OnDragAndDropTask);
        }

        private void OnDisable()
        {
            dragAndDropServiceDrawler.OnDragAndDrop -= OnDragAndDropService;
            dragAndDropElementDrawler.OnDragAndDrop -= OnDragAndDropElement;
            dragAndDropInitTaskDrawler.OnDragAndDrop -= OnDragAndDropTask;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space(2);
            EditorGUILayout.PropertyField(autoRun);

            EditorGUILayout.Space(4);
            GUI.enabled = false;
            EditorGUILayout.LabelField($"Status:  {gameContext.CurrentState}");
            GUI.enabled = true;

            EditorGUILayout.Space(2);
            DrawGameServices();
            EditorGUILayout.Space(10);
            DrawGameElements();
            EditorGUILayout.Space(10);
            DrawInitTasks();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawGameServices()
        {
            EditorGUILayout.PropertyField(gameServices, includeChildren: true);
            EditorGUILayout.Space(8);
            dragAndDropServiceDrawler.Draw();
        }

        private void DrawGameElements()
        {
            EditorGUILayout.PropertyField(gameElements, includeChildren: true);
            EditorGUILayout.Space(8);
            dragAndDropElementDrawler.Draw();
        }

        private void DrawInitTasks()
        {
            EditorGUILayout.PropertyField(constructTasks, includeChildren: true);
            EditorGUILayout.Space(8);
            dragAndDropInitTaskDrawler.Draw();
        }

        private void OnDragAndDropElement(Object draggedObject)
        {
            if (draggedObject is GameObject gameObject)
            {
                AddElementByGameObject(gameObject);
                EditorUtility.SetDirty(gameContext);
            }

            if (draggedObject is IGameElement gameElement)
            {
                gameContext.Editor_AddElement((MonoBehaviour) gameElement);
                EditorUtility.SetDirty(gameContext);
            }
        }

        private void AddElementByGameObject(GameObject gameObject)
        {
            var gameElements = gameObject.GetComponents<IGameElement>();
            foreach (var element in gameElements)
            {
                gameContext.Editor_AddElement((MonoBehaviour) element);
            }
        }

        private void OnDragAndDropService(Object draggedObject)
        {
            if (draggedObject is GameObject gameObject)
            {
                AddServiceByGameObject(gameObject);
                EditorUtility.SetDirty(gameContext);
            }

            if (draggedObject is MonoBehaviour monoBehaviour)
            {
                AddServiceByMonoBehavour(monoBehaviour);
                EditorUtility.SetDirty(gameContext);
            }
        }

        private void AddServiceByMonoBehavour(MonoBehaviour monoBehaviour)
        {
            if (monoBehaviour is IGameServiceGroup)
            {
                gameContext.Editor_AddService(monoBehaviour);
                return;
            }

            if (monoBehaviour is not IGameElementGroup)
            {
                gameContext.Editor_AddService(monoBehaviour);
            }
        }

        private void AddServiceByGameObject(GameObject gameObject)
        {
            var monoBehaviours = gameObject.GetComponents<MonoBehaviour>();
            foreach (var monoBehaviour in monoBehaviours)
            {
                AddServiceByMonoBehavour(monoBehaviour);
            }
        }
        
        private void OnDragAndDropTask(Object draggedObject)
        {
            if (draggedObject is GameContext.ConstructTask task)
            {
                AddInitTask(task);
                EditorUtility.SetDirty(gameContext);
            }
        }

        private void AddInitTask(GameContext.ConstructTask task)
        {
            gameContext.Editor_AddConstructTask(task);
        }
    }
}
#endif