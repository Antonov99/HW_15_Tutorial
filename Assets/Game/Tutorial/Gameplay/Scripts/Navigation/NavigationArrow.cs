using UnityEngine;

namespace Game.Tutorial.Gameplay
{
    public sealed class NavigationArrow : MonoBehaviour
    {
        [SerializeField]
        private GameObject rootGameObject;

        [SerializeField]
        private Transform rootTransform;

        public void Show()
        {
            rootGameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);   
        }

        public void SetPosition(Vector3 position)
        {
            rootTransform.position = position;
        }
        
        public void LookAt(Vector3 targetPosition)
        {
            var distanceVector = targetPosition - rootTransform.position;
            distanceVector.y = 0;
            rootTransform.rotation = Quaternion.LookRotation(distanceVector.normalized, Vector3.up);
        }
    }
}