using Game.GameEngine;
using UnityEngine;

namespace Game.Meta
{
    [RequireComponent(typeof(Collider))]
    public sealed class DialogueTrigger : MonoBehaviour
    {
        public DialogueConfig Dialogue
        {
            get { return dialogue; }
        }

        [SerializeField]
        private DialogueConfig dialogue;
    }
}