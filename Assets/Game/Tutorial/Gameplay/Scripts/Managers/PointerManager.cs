using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Tutorial.Gameplay
{
    public sealed class PointerManager : MonoBehaviour
    {
        [SerializeField]
        public GameObject pointer;

        private void Awake()
        {
            pointer.SetActive(false);
        }

        [Button]
        public void ShowPointer(Transform targetPoint)
        {
            ShowPointer(targetPoint.position, targetPoint.rotation);
        }

        public void ShowPointer(Vector3 position, Quaternion rotation)
        {
            var pointerTransform = pointer.transform;
            pointerTransform.position = position;
            pointerTransform.rotation = rotation;
            
            pointer.SetActive(true);
        }

        public void HidePointer()
        {
            pointer.SetActive(false);
        }
    }
}