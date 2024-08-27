using System;
using Game.GameEngine;
using UnityEngine;
using static Game.Meta.IDialoguePresentationModel;

namespace Game.Meta
{
    public sealed class DialoguePresentationModel : IDialoguePresentationModel
    {
        public event Action OnStateChanged;

        public event Action OnFinished;

        public Sprite Icon
        {
            get { return dialogue.Icon; }
        }

        public string CurrentMessage
        {
            get { return dialogue.CurrentMessage; }
        }

        public IOption[] CurrentOptions
        {
            get { return currentOptions; }
        }

        private readonly Dialogue dialogue;

        private IOption[] currentOptions;

        public DialoguePresentationModel(Dialogue dialogue)
        {
            this.dialogue = dialogue;

            UpdateOptions();
        }

        private void UpdateOptions()
        {
            var choices = dialogue.CurrentChoices;
            var count = choices.Length;

            currentOptions = new IOption[count];
            for (var i = 0; i < count; i++)
            {
                var choice = choices[i];
                var option = new Option(this, choice, i);
                currentOptions[i] = option;
            }
        }

        private void SelectChoice(int choiceIndex)
        {
            if (dialogue.MoveNext(choiceIndex))
            {
                UpdateOptions();
                OnStateChanged?.Invoke();
            }
            else
            {
                OnFinished?.Invoke();
            }
        }

        private sealed class Option : IOption
        {
            string IOption.Text
            {
                get { return choice; }
            }

            private readonly DialoguePresentationModel parent;

            private readonly string choice;
            
            private readonly int index;
            
            public Option(DialoguePresentationModel parent, string choice, int index)
            {
                this.parent = parent;
                this.choice = choice;
                this.index = index;
            }

            void IOption.OnSelected()
            {
                parent.SelectChoice(index);
            }
        }
    }
}