using Game.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Gameplay.Conveyors
{
    [AddComponentMenu("Gameplay/Conveyors/Conveyor Info Widget")]
    public sealed class InfoWidget : MonoBehaviour
    {
        public ProgressBar ProgressBar
        {
            get { return progressBar; }
        }

        [SerializeField]
        private GameObject root;

        [SerializeField]
        private Image inputImage;

        [SerializeField]
        private Image outputImage;
        
        [SerializeField]
        private ProgressBar progressBar;

        public void SetVisible(bool isVisible)
        {
            root.SetActive(isVisible);
        }

        public void SetInputIcon(Sprite icon)
        {
            inputImage.sprite = icon;
        }

        public void SetOutputIcon(Sprite icon)
        {
            outputImage.sprite = icon;
        }
    }
}