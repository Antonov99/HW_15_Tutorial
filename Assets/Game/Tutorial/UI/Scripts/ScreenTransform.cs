using UnityEngine;

namespace Game.Tutorial.UI
{
    public sealed class ScreenTransform : MonoBehaviour
    {
        public Transform Value
        {
            get { return rootTransform; }
        }

        [SerializeField]
        private Transform rootTransform;
    }
}