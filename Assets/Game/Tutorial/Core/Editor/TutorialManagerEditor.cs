#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Game.Tutorial.UnityEditor
{
    [CustomEditor(typeof(TutorialManager))]
    public sealed class TutorialManagerEditor : Editor
    {
        private SerializedProperty stepList;

        private TutorialManager manager;

        private void Awake()
        {
            stepList = serializedObject.FindProperty(nameof(stepList));
            manager = (TutorialManager) target;
        }

        public override void OnInspectorGUI()
        {
            if (EditorApplication.isPlaying)
            {
                GUI.enabled = false;
                EditorGUILayout.Toggle("Completed", manager.IsCompleted);
                EditorGUILayout.EnumPopup("Current Step", manager.CurrentStep);
                GUI.enabled = true;
                
                EditorGUILayout.Space(8);
                if (GUILayout.Button("Move Next"))
                {
                    manager.FinishCurrentStep();
                    manager.MoveToNextStep();
                }
            }
            
            EditorGUILayout.Space(4.0f);
            EditorGUILayout.PropertyField(stepList, includeChildren: true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif