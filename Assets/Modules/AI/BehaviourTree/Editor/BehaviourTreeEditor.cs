#if UNITY_EDITOR
using UnityEditor;

namespace AI.BTree.UnityEditor
{
    [CustomEditor(typeof(UnityBehaviourTree))]
    public sealed class BehaviourTreeEditor : Editor
    {
        private UnityBehaviourTree behaviourTree;

        private SerializedProperty root;

        private SerializedProperty updateMode;

        private SerializedProperty autoRun;

        private SerializedProperty loop;

        private void Awake()
        {
            behaviourTree = (UnityBehaviourTree) target;
            root = serializedObject.FindProperty(nameof(root));
            updateMode = serializedObject.FindProperty(nameof(updateMode));
            autoRun = serializedObject.FindProperty(nameof(autoRun));
            loop = serializedObject.FindProperty(nameof(loop));
        }

        public override void OnInspectorGUI()
        {
            if (behaviourTree.Editor_GetRoot() == null)
            {
                EditorGUILayout.HelpBox("Root is not installed", MessageType.Error, false);
            }
            else
            {
                InspectorHelper.DrawRunningParameter(behaviourTree.IsRunning);
            }

            EditorGUILayout.Space(2.0f);

            EditorGUILayout.PropertyField(autoRun);
            EditorGUILayout.PropertyField(loop);
            
            if (loop.boolValue)
            {
                EditorGUILayout.PropertyField(updateMode);
            }


            EditorGUILayout.Space(2.0f);
            EditorGUILayout.PropertyField(root, true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif