using System;
using UnityEngine;

namespace Game.GameEngine
{
    public sealed class Dialogue
    {
        public Sprite Icon
        {
            get { return config.icon; }
        }
        
        public string CurrentMessage
        {
            get { return currentNode.content; }
        }

        public string[] CurrentChoices
        {
            get { return currentNode.choices; }
        }

        private readonly DialogueConfig config;

        private DialogueConfig.Node currentNode;

        public Dialogue(DialogueConfig config)
        {
            if (!config.FindEntryNode(out var node))
            {
                throw new Exception("Entry point is absent!");
            }

            this.config = config;
            currentNode = node;
        }

        public bool MoveNext(int choiceIndex)
        {
            if (config.FindNextNode(currentNode.id, choiceIndex, out var nextNode))
            {
                currentNode = nextNode;
                return true;
            }

            return false;
        }
    }
}