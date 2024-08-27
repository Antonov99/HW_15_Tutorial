using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public sealed class PointMarker : MonoBehaviour
    {
        [SerializeField]
        private GameObject root;

        [SerializeField]
        private Image iconImage;

        public void SetIcon(Sprite icon)
        {
            iconImage.sprite = icon;
        }
        
        public void Show()
        {
            root.SetActive(true);
        }

        public void Hide()
        {
            root.SetActive(false);
        }
    }
}