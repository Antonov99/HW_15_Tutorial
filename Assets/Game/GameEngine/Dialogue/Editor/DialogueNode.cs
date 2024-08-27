#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.GameEngine.UnityEditor
{
    public sealed class DialogueNode : Node
    {
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Content
        {
            get { return contentTextField.value; }
            set { contentTextField.value = value; }
        }

        public Port InputPort
        {
            get { return inputPort; }
        }

        public List<Choice> Choices
        {
            get { return choices; }
        }

        public bool IsEntry
        {
            get { return isEntry; }
        }

        private int id;

        private TextField contentTextField;

        private Port inputPort;

        private readonly List<Choice> choices = new();

        private bool isEntry;

        public static DialogueNode Instantiate(Vector2 position)
        {
            var node = new DialogueNode();
            node.InitTitle();
            node.InitBody();
            node.InitButton_AddChoice();
            node.InitStyleSheets();
            node.RefreshExpandedState();
            node.SetPosition(new Rect(position, Vector2.zero));
            return node;
        }

        private void InitTitle()
        {
            inputPort = InstantiatePort(
                Orientation.Horizontal,
                Direction.Input,
                Port.Capacity.Multi,
                typeof(bool)
            );

            inputPort.portColor = Color.white;
            inputPort.portName = "";
            titleContainer.Insert(0, inputPort);
        }

        private void InitBody()
        {
            contentTextField = new TextField
            {
                value = "Message",
                multiline = true
            };
            
            inputContainer.Insert(0, contentTextField);
        }

        private void InitButton_AddChoice()
        {
            var button = new Button
            {
                text = "Add Choice",
                clickable = new Clickable(() => AddChoice("Answer"))
            };

            extensionContainer.Add(button);
        }

        public void AddChoice(string content, bool refresh = true)
        {
            var port = InstantiatePort(
                Orientation.Horizontal,
                Direction.Output,
                Port.Capacity.Single,
                typeof(bool)
            );
            port.portName = "";
            port.AddToClassList("dialogue_node_port");


            var deleteButton = new Button
            {
                text = "-",
                clickable = new Clickable(() => RemoveOutputPort(port))
            };

            var choiceText = new TextField
            {
                value = content
            };

            choiceText.AddToClassList("dialogue_node_choice");

            port.Add(deleteButton);
            port.Add(choiceText);

            outputContainer.Add(port);

            var result = new Choice
            {
                port = port,
                textField = choiceText
            };
            choices.Add(result);


            if (refresh)
            {
                RefreshExpandedState();
            }
        }

        public void RemoveOutputPort(Port outputPort, bool refresh = true)
        {
            outputContainer.Remove(outputPort);

            var choice = choices.FirstOrDefault(it => it.port == outputPort);
            if (choice.port != null)
            {
                choices.Remove(choice);
            }

            if (refresh)
            {
                RefreshExpandedState();
            }
        }

        public int IndexOfChoice(Port output)
        {
            for (var i = 0; i < choices.Count; i++)
            {
                var choice = choices[i];
                if (choice.port == output)
                {
                    return i;
                }
            }

            throw new Exception("Index of port is not found!");
        }


        private void InitStyleSheets()
        {
            extensionContainer.AddToClassList("dialogue_node_extension-container");
            mainContainer.AddToClassList("dialogue_node_main-container");
            contentTextField.AddToClassList("dialogue_node_message");

            style.borderTopLeftRadius = 8;
            style.borderTopRightRadius = 8;
            style.borderBottomLeftRadius = 8;
            style.borderBottomRightRadius = 8;
        }

        public void SetAsEntry()
        {
            style.backgroundColor = new Color(0.92f, 0.76f, 0f);
            isEntry = true;
        }

        public void SetAsNotEntry()
        {
            style.backgroundColor = new Color(0.53f, 0.53f, 0.56f);
            isEntry = false;
        }

        public struct Choice
        {
            public Port port;
            public TextField textField;
        }
    }
}
#endif