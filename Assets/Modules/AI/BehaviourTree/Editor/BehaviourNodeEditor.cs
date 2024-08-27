#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace AI.BTree.UnityEditor
{
    [CustomEditor(typeof(UnityBehaviourNode), editorForChildClasses: true)]
    public class BehaviourNodeEditor : OdinEditor
    {
        protected UnityBehaviourNode node;

        protected virtual void Awake()
        {
            node = (UnityBehaviourNode) target;
        }

        public override void OnInspectorGUI()
        {
            InspectorHelper.DrawRunningParameter(node.IsRunning);
            EditorGUILayout.Space(4.0f);
            base.OnInspectorGUI();
        }
    }
}
#endif