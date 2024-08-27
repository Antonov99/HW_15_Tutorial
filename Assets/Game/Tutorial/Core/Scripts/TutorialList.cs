using System.Collections.Generic;
using UnityEngine;

namespace Game.Tutorial
{
    [CreateAssetMenu(
        fileName = "TutorialList",
        menuName = "Tutorial/New TutorialList",
        order = 35
    )]
    public sealed class TutorialList : ScriptableObject
    {
        public int LastIndex
        {
            get { return steps.Count - 1; }
        }

        [SerializeField]
        private List<TutorialStep> steps = new();

        public TutorialStep this[int index]
        {
            get { return steps[index]; }
        }
        
        public int IndexOf(TutorialStep step)
        {
            return steps.IndexOf(step);
        }

        public bool IsLast(int index)
        {
            return index >= steps.Count - 1;
        }
    }
}