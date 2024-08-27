#if UNITY_EDITOR
using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace AI.GOAP.UnityEditor
{
    [CustomEditor(typeof(GoalOrientedAgent))]
    public sealed class GoalOrientedAgentEditor : OdinEditor
    {
        private GoalOrientedAgent agent;
        
        private bool worldStateFoldout = true;

        protected override void OnEnable()
        {
            base.OnEnable();
            agent = (GoalOrientedAgent) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (EditorApplication.isPlaying)
            {
                DrawPlaymode();
            }
        }

        private void DrawPlaymode()
        {
            GUI.enabled = false;
            DrawGoals();
            DrawActions();
            DrawWorldState();
            GUI.enabled = true;
            DrawButtons();
        }

        private void DrawGoals()
        {
            EditorGUILayout.Space(4.0f);
            EditorGUILayout.LabelField("Active Goals");
            var goals = agent.Goals
                .OrderByDescending(it => it.IsValid() ? it.EvaluatePriority() : -1);

            foreach (var goal in goals)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField(goal as Goal, typeof(Goal), false);
                var isValid = goal.IsValid();
                if (isValid)
                {
                    EditorGUILayout.TextField("Priority: " + goal.EvaluatePriority());
                }
                else
                {
                    EditorGUILayout.TextField("Inactive");
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawActions()
        {
            EditorGUILayout.Space(4.0f);
            EditorGUILayout.LabelField("Active Actions");
            var actions = agent.Actions
                .OrderByDescending(it => it.IsValid() ? it.EvaluateCost() : -1);

            foreach (var action in actions)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField(action as Actor, typeof(Actor), false);
                var isValid = action.IsValid();
                if (isValid)
                {
                    EditorGUILayout.TextField("Cost: " + action.EvaluateCost());
                }
                else
                {
                    EditorGUILayout.TextField("Inactive");
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawWorldState()
        {
            GUI.enabled = false;
            EditorGUILayout.Space(16);

            worldStateFoldout = EditorGUILayout.Foldout(worldStateFoldout, "WorldState");
            if (worldStateFoldout)
            {
                EditorGUI.indentLevel++;

                var worldState = agent.WorldState;
                foreach (var (id, value) in worldState)
                {
                    EditorGUILayout.Toggle(id, value);
                }

                EditorGUI.indentLevel--;
            }

            GUI.enabled = true;
        }

        private void DrawButtons()
        {
            EditorGUILayout.Space(8.0f);
            if (GUILayout.Button("Play"))
            {
                agent.Play();
            }

            if (GUILayout.Button("Replay"))
            {
                agent.Replay();
            }

            if (GUILayout.Button("Cancel"))
            {
                agent.Cancel();
            }
        }
    }
}
#endif