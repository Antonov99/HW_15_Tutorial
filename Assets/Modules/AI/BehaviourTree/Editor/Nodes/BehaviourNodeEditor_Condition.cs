#if UNITY_EDITOR
using UnityEditor;

namespace AI.BTree.UnityEditor
{
    [CustomEditor(typeof(UnityBehaviourNode_CheckCondition))]
    public sealed class BehaviourNodeEditor_Condition : Editor
    {
        private UnityBehaviourNode node;

        private SerializedProperty invertCondition;

        private SerializedProperty conditions;

        private void Awake()
        {
            node = (UnityBehaviourNode) target;
            invertCondition = serializedObject.FindProperty(nameof(invertCondition));
            conditions = serializedObject.FindProperty(nameof(conditions));
        }

        public override void OnInspectorGUI()
        {
            InspectorHelper.DrawRunningParameter(node.IsRunning);
            EditorGUILayout.Space(4.0f);
            
            invertCondition.boolValue = EditorGUILayout.Toggle("Invert Condition", invertCondition.boolValue);
            EditorGUILayout.PropertyField(conditions, true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif