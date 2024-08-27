#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Game.GameEngine.UnityEditor
{
    public sealed class DialogueWindow : EditorWindow
    {
        private DialogueGraph graphView;

        private ObjectField dialogField;

        [MenuItem("Window/Dialogue/Dialogue Window")]
        public static void ShowWindow()
        {
            GetWindow<DialogueWindow>("Dialogue Window");
        }

        private void OnEnable()
        {
            InitGraphView();
            InitToolbar();
        }

        private void InitGraphView()
        {
            graphView = new DialogueGraph();
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }

        private void InitToolbar()
        {
            dialogField = new ObjectField("Selected Dialog")
            {
                objectType = typeof(DialogueConfig),
                allowSceneObjects = false
            };

            var loadButton = new Button
            {
                text = "Load",
                clickable = new Clickable(() =>
                {
                    DialogueManager.LoadDialog(graphView, dialogField.value as DialogueConfig);
                })
            };

            var saveButton = new Button
            {
                text = "Save",
                clickable = new Clickable(() =>
                {
                    var config = dialogField.value as DialogueConfig;
                    if (config != null)
                    {
                        DialogueManager.SaveDialog(graphView, config);
                    }
                    else
                    {
                        DialogueManager.CreateDialog(graphView, out config);
                        dialogField.value = config;
                    }
                })
            };

            var toolbar = new Toolbar();
            toolbar.Add(dialogField);
            toolbar.Add(loadButton);
            toolbar.Add(saveButton);

            rootVisualElement.Add(toolbar);
        }
    }
}
#endif